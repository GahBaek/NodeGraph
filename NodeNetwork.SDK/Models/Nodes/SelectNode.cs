using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    // true/false 에 따라 두 값 중 하나를 고르는 노드
    public sealed class SelectNode : INode
    {
        public string Name { get; }
        private readonly string _selectorKey, _out;
        private readonly Dictionary<string, string> _routes;

        public SelectNode(string name, string selectorKey, string outKey, params (string caseValue, string fromKey)[] routes)
        {
            Name = name; 
            _selectorKey = selectorKey; 
            _out = outKey;
            _routes = new Dictionary<string, string>();
            foreach (var (c, k) in routes) _routes[c] = k;
        }

        public IContext Exec(IContext ctx)
        {
            var sel = ctx.Get<object>(_selectorKey)?.ToString() ?? "";
            if (!_routes.TryGetValue(sel, out var fromKey))
                throw new InvalidOperationException($"[{Name}] no route for '{sel}'");
            var v = ctx.Get<object>(fromKey);
            return ctx.Set(_out, v);
        }
    }
}
