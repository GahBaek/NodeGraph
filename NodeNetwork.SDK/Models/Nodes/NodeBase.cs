using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public readonly record struct GraphId(Guid Value);
    // 노드 If 를 의미 있는 타입으로 감싼 핸들
    public readonly record struct NodeHandle(Guid Value);
    public abstract class NodeBase : INode
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public string Name { get; }

        public NodeMeta Meta { get; }

        protected NodeBase(string name, NodeMeta meta) { Name = name; Meta = meta; }

        protected static T Need<T> (IReadOnlyDictionary<string, IValue> map, string key) where T : IValue
        {
            if (!map.TryGetValue(key, out var value) || value is not T t)
                throw new ArgumentException($"Missing/Invalid '{key}'");
            return t;
        }

        public abstract IReadOnlyDictionary<string, IValue> Execute(IReadOnlyDictionary<string, IValue> inputs);
        public virtual INode WithId(Guid id) { Id = id; return this; }
    }
}
