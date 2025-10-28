using System;

namespace NodeNetworkSDK.Models.Values
{
    public sealed class BoolType : IDataType
    {
        public static readonly BoolType Instance = new();
        private BoolType() { }

        public string Id => "core.bool";
        public bool IsAssignableFrom(IDataType other) => ReferenceEquals(this, other);
    }

    public sealed class BoolValue : IValue
    {
        public IDataType Type => BoolType.Instance;

        public bool _v;
        public BoolValue(bool v) { _v = v; }

        public bool Value => _v;

        public static BoolValue Of(bool v) => new(v);

        public static BoolValue True { get; } = new(true);
        public static BoolValue False { get; } = new(false);

        public BoolValue And(BoolValue b) => new(_v && b._v);
        public BoolValue Or(BoolValue b) => new(_v || b._v);
        public BoolValue Xor(BoolValue b) => new(_v ^ b._v);
        public BoolValue Not() => new(!_v);

        public override string ToString() => _v.ToString();
    }
}
