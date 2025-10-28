using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    // 연산 로직
    public interface INode
    {
        public Guid Id { get; }
        string Name { get; }
        NodeMeta Meta { get; }
        public IReadOnlyDictionary<string, IValue> Execute(IReadOnlyDictionary<string, IValue> inputs);
        public INode WithId(Guid id);
    }
}
