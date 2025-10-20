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
        private sealed class Graph { 
            public string Name { get; }
            public Graph(string name) { Name = name; }
            public readonly Dictionary<Guid, INode> Nodes = new();
            public readonly HashSet<(Guid From, string Out, Guid To, string In)> Edges = new();
            // Edge 로 연결되지 않은 입력 포트에 꽂는 상수 값들
            public readonly Dictionary<(Guid Node, string Input), IValue> Literals = new();
        }

        private readonly Dictionary<Guid, Graph> _graphs = new();

        public GraphId CreateGraph(string name) { 
            var g  = new Graph(name);
            var id = new GraphId(Guid.NewGuid());

            _graphs[id.Value] = g;
            return id;
        }
        // GraphId 를 이용해 graph getter
        private Graph G(GraphId id) 
            => _graphs.TryGetValue(id.Value, out var g) ? g : throw new KeyNotFoundException("Graph not found");

        // reflection 기반 노드 생성
        public NodeHandle AddNode(GraphId gid, string nodeClassName, string name) {
            var g = G(gid);
            // node 이름을 통해 node type 반환
            var t = ResolveNodeType(nodeClassName);
            var ctor = t.GetConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, binder: null, new[] { typeof(string) }, modifiers: null)
                ?? throw new MissingMethodException($"{t.FullName} needs a (string) constructor.");

            var node = (INode)(ctor.Invoke(new object[] { name }) 
                ?? throw new InvalidOperationException("Ctor returned null"));

            g.Nodes[node.Id] = node;
            return new NodeHandle(node.Id);
        }

        // 어떤 node 인지 return 하는 메소드
        private static Type ResolveNodeType(string className) {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) { 
                var t = asm.GetType(className, throwOnError: false, ignoreCase: false);
                if (IsValidNodeType(t))
                    return t;
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                var t = asm.GetTypes().FirstOrDefault(x => x.Name == className && IsValidNodeType(x));
                if (t != null)
                    return t;
            }

            throw new TypeLoadException($"Node type '{className}' not found or invalid.");
        }

        private static bool IsValidNodeType(Type? t) => t != null && typeof(INode).IsAssignableFrom(t);

        // node 의 meta getter
        public NodeMeta GetNodeMeta(GraphId gid, NodeHandle node) => G(gid).Nodes[node.Value].Meta;

        public void SetInput(IContext ctx, NodeHandle node, string inputName, IValue value, GraphId gid) {
            var g = G(gid);
            var meta = g.Nodes[node.Value].Meta;
            var spec = meta.Inputs.FirstOrDefault(p => p.Name == inputName)
                ?? throw new ArgumentException($"No such input '{inputName}'");

            if(!spec.Type.IsAssignableFrom(value.Type))
                throw new ArgumentException($"Type mismatch for '{inputName}' (need {spec.Type.Id}, got {value.Type.Id})");

            g.Literals[(node.Value, inputName)] = value;
        }

        // node 간 연결 관리
        public bool Connect(GraphId gid, NodeHandle from, string fromOutput, NodeHandle to, string toInput) {
            var g = G(gid);
            if(!g.Nodes.TryGetValue(from.Value, out var src) || !g.Nodes.TryGetValue(to.Value, out var dst))
                return false;

            var outSpec = src.Meta.Outputs.FirstOrDefault(p => p.Name == fromOutput);
            var inSpec = dst.Meta.Inputs.FirstOrDefault(p => p.Name == toInput);
            if(outSpec is null || inSpec is null)
                return false;
            if (!inSpec.Type.IsAssignableFrom(outSpec.Type))
                return false;

            g.Edges.Add((from.Value, fromOutput, to.Value, toInput));
            return true;
        }
        public bool Disconnect(GraphId gid, NodeHandle from, string fromOutput, NodeHandle to, string toInput) =>
            G(gid).Edges.Remove((from.Value, fromOutput, to.Value, toInput));

        public (bool OK, IEnumerable<string> Errors) Validate(GraphId gid, IContext ctx) { }

        public void Execute(GraphId gid, IContext ctx) { }
    }
}
