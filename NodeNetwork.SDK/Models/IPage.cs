using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeNetwork.SDK.Models
{
    public interface IPage   
    {
        Guid Id { get; }
        string Name { get; }

        // 노드/엣지 관리
        Guid AddNode(INode node);
        bool RemoveNode(Guid nodeId);

        bool Connect(Guid fromNodeId, string fromPortKey, Guid toNodeId, string toPortKey);
        bool Disconnect(Guid fromNodeId, string fromPortKey, Guid toNodeId, string toPortKey);

        // 검증/실행
        GraphValidationResult Validate(); 
        IContext Exec(IContext ctx);
    }


    public sealed class Page : IPage
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string ResultKey { get; }
        private readonly Dictionary<Guid, INode> _nodes = new();
        private readonly HashSet<Edge> _edges = new();

        public Page(string name, string resultKey)
        {
            Name = name;
            ResultKey = resultKey;
        }

        public Guid AddNode(INode node) { _nodes[node.Id] = node; return node.Id; }
        public bool RemoveNode(Guid nodeId)
        {
            var removed = _nodes.Remove(nodeId);
            if (removed)
            {
                _edges.RemoveWhere(e => e.FromNodeId == nodeId || e.ToNodeId == nodeId);
            }
            return removed;
        }

        public bool Connect(Guid fromId, string fromKey, Guid toId, string toKey)
        {
            if (!_nodes.ContainsKey(fromId) || !_nodes.ContainsKey(toId)) return false;
            if (!_nodes[fromId].Outputs.Contains(fromKey) || !_nodes[toId].Inputs.Contains(toKey)) return false;

            var edge = new Edge(Guid.NewGuid(), fromId, fromKey, toId, toKey);
            return _edges.Add(edge);
        }

        public bool Disconnect(Guid fromId, string fromKey, Guid toId, string toKey)
        {
            var existing = _edges.FirstOrDefault(e =>
                e.FromNodeId == fromId && e.FromPortKey == fromKey &&
                e.ToNodeId == toId && e.ToPortKey == toKey);

            if (existing is null) return false;
            return _edges.Remove(existing);
        }

        public GraphValidationResult Validate()
        {
            foreach (var e in _edges)
            {
                if (!_nodes.ContainsKey(e.FromNodeId) || !_nodes.ContainsKey(e.ToNodeId))
                    return GraphValidationResult.Fail($"edge contains unknown node: {e}");
                if (!_nodes[e.FromNodeId].Outputs.Contains(e.FromPortKey))
                    return GraphValidationResult.Fail($"unknown out port '{e.FromPortKey}' on node {e.FromNodeId}");
                if (!_nodes[e.ToNodeId].Inputs.Contains(e.ToPortKey))
                    return GraphValidationResult.Fail($"unknown in port '{e.ToPortKey}' on node {e.ToNodeId}");
            }

            var order = TopoSort();
            if (order is null) return GraphValidationResult.Fail("cycle detected");

            return GraphValidationResult.Ok(order);
        }

        public IContext Exec(IContext ctx)
        {
            var validation = Validate();
            if (!validation.IsValid)
                throw new InvalidOperationException($"Graph invalid: {validation.Error}");

            var current = ctx;

            foreach (var nodeId in validation.Order!)   
                current = _nodes[nodeId].Exec(current);

            if (current.TryGet<object?>(ResultKey, out var v))
                current.SetResult(v);

            return current;
        }


        // 위상 정렬
        private IReadOnlyList<Guid>? TopoSort()
        {
            var indeg = _nodes.Keys.ToDictionary(id => id, _ => 0);
            foreach (var e in _edges)
                indeg[e.ToNodeId]++;

            var q = new Queue<Guid>(_nodes.Keys.Where(id => indeg[id] == 0));
            var order = new List<Guid>();

            while (q.Count > 0)
            {
                var u = q.Dequeue();
                order.Add(u);
                foreach (var e in _edges.Where(x => x.FromNodeId == u))
                {
                    indeg[e.ToNodeId]--;
                    if (indeg[e.ToNodeId] == 0) q.Enqueue(e.ToNodeId);
                }
            }

            return order.Count == _nodes.Count ? order : null;
        }
    }

    public sealed class GraphValidationResult
    {
        public bool IsValid { get; }
        public string? Error { get; }
        public IReadOnlyList<Guid>? Order { get; }

        private GraphValidationResult(bool ok, string? err, IReadOnlyList<Guid>? order)
        { IsValid = ok; Error = err; Order = order; }

        public static GraphValidationResult Ok(IReadOnlyList<Guid> order) => new(true, null, order);
        public static GraphValidationResult Fail(string err) => new(false, err, null);
    }

}
