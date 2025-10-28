using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Serializer
{
    public interface IValueCodec
    {
        string TypeId { get; }
        object ToPayload(IValue v);
        IValue FromPayload(object payload);
    }
    public sealed class NumberCodec : IValueCodec
    {
        public string TypeId => "number";
        public object ToPayload(IValue v) => ((NumberValue)v)._v;
        public IValue FromPayload(object payload) => new NumberValue(Convert.ToDouble(payload));
    }
    public sealed class BoolCodec : IValueCodec
    {
        public string TypeId => "bool";
        public object ToPayload(IValue v) => ((BoolValue)v).Value;
        public IValue FromPayload(object payload) => new BoolValue(Convert.ToBoolean(payload));
    }

    public sealed class ValueCodecRegistry
    {
        private readonly Dictionary<string, IValueCodec> _byId = new();
        public void Register(IValueCodec codec) => _byId[codec.TypeId] = codec;
        public bool TryGet(string typeId, out IValueCodec? c) => _byId.TryGetValue(typeId, out c);
    }
}
