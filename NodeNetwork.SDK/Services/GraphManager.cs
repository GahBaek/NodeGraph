using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Services
{
    public sealed class GraphManager
    {
        public sealed class Graph
        {
            public string Name { get; }
            public Graph(string name) { Name = name; }
            public readonly Dictionary<Guid, INode> Nodes = new();
            public readonly HashSet<(Guid From, string Out, Guid To, string In)> Edges = new();
            // Edge 로 연결되지 않은 입력 포트에 꽂는 상수 값들
            public readonly Dictionary<(Guid Node, string Input), IValue> Literals = new();
        }

        private readonly Dictionary<Guid, Graph> _graphs = new();

        public GraphId CreateGraph(string name)
        {
            var g = new Graph(name);
            var id = new GraphId(Guid.NewGuid());

            _graphs[id.Value] = g;
            return id;
        }
        // GraphId 를 이용해 graph getter
        private Graph G(GraphId id)
            => _graphs.TryGetValue(id.Value, out var g) ? g : throw new KeyNotFoundException("Graph not found");

        // reflection 기반 노드 생성
        // reflection 은 유연한 동적 구성/발견을 가능하게 한다.
        // 성능, 안정성 비용이 있다.
        public NodeHandle AddNode(GraphId gid, string nodeClassName, string name)
        {
            var g = G(gid);
            // node 이름을 통해 node type 반환
            var t = ResolveNodeType(nodeClassName);
            // instance 생성자 중 public, 비공개 모두에서 string 하나를 받는 생성자를 찾는다는 의미
            var ctor = t.GetConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, binder: null, new[] { typeof(string) }, modifiers: null)
                ?? throw new MissingMethodException($"{t.FullName} needs a (string) constructor.");

            var node = (INode)(ctor.Invoke(new object[] { name })
                ?? throw new InvalidOperationException("Ctor returned null"));

            g.Nodes[node.Id] = node;
            return new NodeHandle(node.Id);
        }

        // 어떤 node 인지 return 하는 메소드
        private static Type ResolveNodeType(string className)
        {
            // [AppDomain]
            // .NET 런타임 안에서 어셈블리들이 로드되어 실행되는 격리된 실행 영역을 가리킨다.
            // 여러 가지 런타임 상태가 이 경계 안에 묶여 있다.

            // 현재 앱 도메인에 로드된 assembly들을 열거한다.
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetType(className, throwOnError: false, ignoreCase: false);
                if (IsValidNodeType(t))
                    return t;
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetTypes().FirstOrDefault(x => x.Name == className && IsValidNodeType(x));
                if (t != null)
                    return t;
            }

            throw new TypeLoadException($"Node type '{className}' not found or invalid.");
        }

        private static bool IsValidNodeType(Type? t)
            => t != null && typeof(INode).IsAssignableFrom(t) && !t.IsAbstract;

        // node 의 meta getter
        public NodeMeta GetNodeMeta(GraphId gid, NodeHandle node) => G(gid).Nodes[node.Value].Meta;

        public void SetInput(IContext ctx, NodeHandle node, string inputName, IValue value, GraphId gid)
        {
            var meta = GetNodeMeta(gid, node);
            var spec = meta.Inputs.FirstOrDefault(i => i.Name == inputName)
                       ?? throw new ArgumentException($"No such input '{inputName}'");
            if (!spec.Type.IsAssignableFrom(value.Type))
                throw new ArgumentException($"Type mismatch for '{inputName}' (need {spec.Type.Id}, got {value.Type.Id})");
            ctx.SetInput(node, inputName, value);
            var g = G(gid);
            g.Literals[(node.Value, inputName)] = value;
        }

        // node 간 연결 관리
        public bool Connect(GraphId gid, NodeHandle from, string fromOutput, NodeHandle to, string toInput)
        {
            var g = G(gid);
            if (!g.Nodes.TryGetValue(from.Value, out var src) || !g.Nodes.TryGetValue(to.Value, out var dst))
                return false;

            var outSpec = src.Meta.Outputs.FirstOrDefault(p => p.Name == fromOutput);
            var inSpec = dst.Meta.Inputs.FirstOrDefault(p => p.Name == toInput);
            if (outSpec is null || inSpec is null)
                return false;
            if (!inSpec.Type.IsAssignableFrom(outSpec.Type))
                return false;

            g.Edges.Add((from.Value, fromOutput, to.Value, toInput));
            return true;
        }


        public bool Disconnect(GraphId gid, NodeHandle from, string fromOutput, NodeHandle to, string toInput) =>
            G(gid).Edges.Remove((from.Value, fromOutput, to.Value, toInput));

        public (bool OK, IEnumerable<string> Errors) Validate(GraphId gid, IContext ctx)
        {
            var g = G(gid);
            var errs = new List<string>();

            var indeg = g.Nodes.Keys.ToDictionary(id => id, _ => 0);
            foreach (var e in g.Edges) indeg[e.To]++;
            var q = new Queue<Guid>(g.Nodes.Keys.Where(id => indeg[id] == 0));
            var seen = new List<Guid>();
            while (q.Count > 0)
            {
                var u = q.Dequeue();
                seen.Add(u);
                foreach (var v in g.Edges.Where(x => x.From == u).Select(x => x.To))
                {
                    indeg[v]--;
                    if (indeg[v] == 0) q.Enqueue(v);
                }
            }
            if (seen.Count != g.Nodes.Count) errs.Add("Cycle detected");

            foreach (var (id, n) in g.Nodes)
            {
                foreach (var p in n.Meta.Inputs.Where(p => p.Required))
                {
                    var hasEdge = g.Edges.Any(e => e.To == id && e.In == p.Name);

                    var hasLit = ctx.TryGetInput(new NodeHandle(id), p.Name, out _)
                        || g.Literals.ContainsKey((id, p.Name));

                    if (!hasEdge && !hasLit)
                        errs.Add($"Missing input: {n.Name}.{p.Name}");
                }
            }

            return (!errs.Any(), errs);
        }

        internal Graph Debug_GetGraphSnapshot(GraphId gid) => G(gid);
        internal INode Debug_GetNode(GraphId gid, Guid id) => G(gid).Nodes[id];

        internal void Debug_ReassignNodeIds(GraphId gid,
            Dictionary<Guid, NodeHandle> newlyCreated, Guid[] targetIds)
        {
            var g = G(gid);
            var newIds = newlyCreated.Values.Select(h => h.Value).ToArray();
            if (newIds.Length != targetIds.Length) throw new InvalidOperationException();

            var map = new Dictionary<Guid, Guid>();
            for (int i = 0; i < newIds.Length; i++) map[newIds[i]] = targetIds[i];

            var clones = new Dictionary<Guid, INode>();
            foreach (var (oldId, node) in g.Nodes)
            {
                var nid = map.TryGetValue(oldId, out var want) ? want : oldId;
                clones[nid] = node.WithId(nid);
            }
            g.Nodes.Clear();
            foreach (var kv in clones) g.Nodes[kv.Key] = kv.Value;

            var re = new HashSet<(Guid From, string Out, Guid To, string In)>();
            foreach (var e in g.Edges)
            {
                var from = map.TryGetValue(e.From, out var f2) ? f2 : e.From;
                var to = map.TryGetValue(e.To, out var t2) ? t2 : e.To;
                re.Add((from, e.Out, to, e.In));
            }
            g.Edges.Clear(); foreach (var e in re) g.Edges.Add(e);

            var rl = new Dictionary<(Guid Node, string Input), IValue>();
            foreach (var kv in g.Literals)
            {
                var node = map.TryGetValue(kv.Key.Node, out var n2) ? n2 : kv.Key.Node;
                rl[(node, kv.Key.Input)] = kv.Value;
            }
            g.Literals.Clear(); foreach (var kv in rl) g.Literals[kv.Key] = kv.Value;
        }

        public void Execute(GraphId gid, IContext ctx)
        {
            var g = G(gid);

            if (ctx is not Context c)
                throw new ArgumentException("Context must be instance of Context class.");

            var (ok, errs) = Validate(gid, ctx);
            if (!ok)
            {
                Console.WriteLine("== Graph validation failed ==");
                foreach (var e in errs) Console.WriteLine(" - " + e);
                throw new InvalidOperationException("Graph invalid: " + string.Join("; ", errs));
            }

            var indeg = g.Nodes.Keys.ToDictionary(id => id, _ => 0);
            foreach (var e in g.Edges) indeg[e.To]++;
            var q = new Queue<Guid>(g.Nodes.Keys.Where(id => indeg[id] == 0));
            var order = new List<Guid>();
            while (q.Count > 0)
            {
                var u = q.Dequeue(); order.Add(u);
                foreach (var v in g.Edges.Where(x => x.From == u).Select(x => x.To))
                { indeg[v]--; if (indeg[v] == 0) q.Enqueue(v); }
            }

            var outCache = new Dictionary<(Guid Node, string Out), IValue>();
            foreach (var id in order)
            {
                var node = g.Nodes[id];
                var inputs = new Dictionary<string, IValue>();

                foreach (var p in node.Meta.Inputs)
                {
                    if (c._inputs.TryGetValue((id, p.Name), out var lit)) { inputs[p.Name] = lit; continue; }

                    if (g.Literals.TryGetValue((id, p.Name), out var glit)) { inputs[p.Name] = glit; continue; }

                    var e = g.Edges.FirstOrDefault(x => x.To == id && x.In == p.Name);
                    if (!e.Equals(default((Guid, string, Guid, string))))
                    {
                        if (outCache.TryGetValue((e.From, e.Out), out var val)) { inputs[p.Name] = val; continue; }
                        throw new InvalidOperationException($"Upstream not ready for {node.Name}.{p.Name}");
                    }

                    if (p.Required) throw new InvalidOperationException($"Missing input at runtime: {node.Name}.{p.Name}");
                }

                var nodeOut = node.Execute(inputs);
                foreach (var (k, v) in nodeOut)
                {
                    outCache[(id, k)] = v;
                    c.SetOutput(id, k, v);
                }
            }
        }
    }
}
