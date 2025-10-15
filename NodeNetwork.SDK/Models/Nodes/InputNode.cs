using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public sealed class InputNode : INode
    {
        private readonly string _key;
        private readonly Func<object> _provider;
        public string Name { get; }
        public InputNode(string name, string key, Func<object> provider)
        { Name = name; _key = key; _provider = provider; }

        public IContext Exec(IContext ctx)
            => ctx.Set(_key, _provider());
    }

}
