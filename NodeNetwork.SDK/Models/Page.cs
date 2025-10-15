using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public sealed class Page : IPage
    {
        private readonly List<INode> _nodes = new();
        public IEnumerable<INode> EnumerateNodes() => _nodes;
        public string Name { get; }
        public string ResultKey { get; }
        public Page(string name, string resultKey) { Name = name; ResultKey = resultKey; }

        public void AddNode(INode node) => _nodes.Add(node);

        public IContext Exec(IContext ctx)
        {
            foreach (var n in _nodes) ctx = n.Exec(ctx);
            if (ctx.TryGet<object>(ResultKey, out var res)) ctx.SetResult(res);
            return ctx;
        }

        public List<INode> GetNodeList()
        {
            return this._nodes;
        }
    }
}
