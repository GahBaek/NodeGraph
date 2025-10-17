using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public enum Direction
    {
        In, Out
    }
    public sealed class Page : IPage
    {
        private readonly Dictionary<Guid, INode> _nodes = new();
        private readonly HashSet<Edge> _edges = new();
        // public IEnumerable<INode> EnumerateNodes() => _nodes;
        public string Name { get; }
        public string ResultKey { get; }
        public Page(string name, string resultKey) { Name = name; ResultKey = resultKey; }

        public INode AddNode(INode node)
        {
            _nodes[node.Id]= node;
            return node;
        }

        public IContext Exec(IContext ctx)
        {
            foreach (var n in _nodes.Values)
            {
                ctx = n.Exec(ctx);
                Console.WriteLine(n.Name);
            }
                

            if (ctx.TryGet<object>(ResultKey, out var res))
                ctx = ctx.SetResult(res);

            return ctx;
        }

        public Dictionary<Guid, INode> GetNodeList()
        {
            return this._nodes;
        }

        public bool Connect(Guid fromId, string fromPort, Guid toId, string toPort)
        {
            if (!_nodes.TryGetValue(fromId, out var fromNode)) return false;
            if (!_nodes.TryGetValue(toId, out var toNode)) return false;


            var edge = new Edge(fromId, fromPort, toId, toPort);
            return _edges.Add(edge);
        }


        public bool Disconnect(Guid fromId, string fromPort, Guid toId, string toPort)
            => _edges.Remove(new Edge(fromId, fromPort, toId, toPort));

        private static bool PortExistsAndMatches(INode node, string portKey, Direction dir)
        {
            if (dir == Direction.Out)
                return node.Outputs != null && node.Outputs.TryGetValue(portKey, out _);
            else
                return node.Inputs != null && node.Inputs.TryGetValue(portKey, out _);
        }


    }
}