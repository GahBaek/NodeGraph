using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Services.Serializer
{
    // Literal: 영구저장을 위함.
    public interface IValueCodec
    {
        string TypeId { get; }
        object ToPayload(IValue v);
        IValue FromPayload(object payload);
    }
    public sealed class NumberCodec : IValueCodec
    {
        public string TypeId => "core.number";
        public object ToPayload(IValue v) => ((NumberValue)v)._v;
        public IValue FromPayload(object payload)
        {
            switch (payload)
            {
                case decimal d: return new NumberValue(d);
                case double d: return new NumberValue(d);
                case float f: return new NumberValue((double)f);
                case string s when decimal.TryParse(s, out var dd): return new NumberValue(dd);
                case System.Text.Json.JsonElement je:
                    if (je.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        // 값 손실 없이 decimal로
                        if (je.TryGetDecimal(out var dec)) return new NumberValue(dec);
                        if (je.TryGetDouble(out var dbl)) return new NumberValue(dbl);
                    }
                    if (je.ValueKind == System.Text.Json.JsonValueKind.String &&
                        decimal.TryParse(je.GetString(), out var dec2)) return new NumberValue(dec2);
                    break;
            }
            throw new InvalidOperationException($"Unsupported payload for core.number: {payload?.GetType().Name ?? "null"}");
        }
    }
    public sealed class BoolCodec : IValueCodec
    {
        public string TypeId => "core.bool";
        public object ToPayload(IValue v) => ((BoolValue)v).Value;
        public IValue FromPayload(object payload)
        {
            switch (payload)
            {
                case bool b: return new BoolValue(b);
                case string s when bool.TryParse(s, out var bb): return new BoolValue(bb);
                case System.Text.Json.JsonElement je:
                    if (je.ValueKind == System.Text.Json.JsonValueKind.True) return new BoolValue(true);
                    if (je.ValueKind == System.Text.Json.JsonValueKind.False) return new BoolValue(false);
                    if (je.ValueKind == System.Text.Json.JsonValueKind.String &&
                        bool.TryParse(je.GetString(), out var b2)) return new BoolValue(b2);
                    break;
            }
            throw new InvalidOperationException($"Unsupported payload for core.bool: {payload?.GetType().Name ?? "null"}");
        }
    }

    public sealed class ValueCodecRegistry
    {
        private readonly Dictionary<string, IValueCodec> _byId =
            new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _alias =
            new(StringComparer.OrdinalIgnoreCase);

        public void Register(IValueCodec codec, params string[] aliases)
        {
            _byId[codec.TypeId] = codec;
            foreach (var a in aliases) _alias[a] = codec.TypeId;
        }

        public void RegisterAlias(string alias, string canonicalTypeId)
            => _alias[alias] = canonicalTypeId;

        public bool TryGet(string typeId, out IValueCodec? c)
        {
            if (_byId.TryGetValue(typeId, out c)) return true;

            if (_alias.TryGetValue(typeId, out var canonical) &&
                _byId.TryGetValue(canonical, out c)) return true;

            var idx = typeId.LastIndexOf('.');
            if (idx >= 0)
            {
                var tail = typeId[(idx + 1)..];
                if (_byId.TryGetValue(tail, out c)) return true;
                if (_alias.TryGetValue(tail, out canonical) &&
                    _byId.TryGetValue(canonical, out c)) return true;
            }

            c = null;
            return false;
        }
    }


}
