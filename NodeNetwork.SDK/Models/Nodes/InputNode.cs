using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    // 값 입력 받는 노드
    public sealed class InputNode : INode
    {
        private readonly string _key;
        public Guid Id { get; }

        private static readonly IReadOnlyDictionary<string, Port> _inputSpec =
            new Dictionary<string, Port>
            {
            };

        private static readonly IReadOnlyDictionary<string, Port> _outputSpec =
            new Dictionary<string, Port>
            {
                ["out"] = new Port(typeof(object))
            };

        public IReadOnlyDictionary<string, Port> Inputs => _inputSpec;
        public IReadOnlyDictionary<string, Port> Outputs => _outputSpec;

        private readonly Func<object> _provider;
        public string Name { get; }
        public InputNode(string name, string key, Func<object> provider)
        { Name = name; _key = key; _provider = provider; Id = Guid.NewGuid(); }

        public IContext Exec(IContext ctx)
            => ctx.Set(_key, _provider());
    }

}
