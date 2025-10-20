using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Values
{
    public sealed class NumberType : IDataType
    {
        public static readonly NumberType Instance = new();
        private NumberType() { }
        public string Id => "core.number";
        public bool IsAssignableFrom(IDataType other) => ReferenceEquals(this, other);
    }

    public sealed class NumberValue : IValue
    {
        public IDataType Type => NumberType.Instance;
        private readonly decimal _v;
        private NumberValue(decimal v) { _v = v; }

        public static NumberValue Of(int v) => new(v);
        public static NumberValue Of(double v) => new((decimal)v);
        public static NumberValue Of(decimal v) => new(v);

        public NumberValue Add(NumberValue b) => new(_v + b._v);
        public NumberValue Sub(NumberValue b) => new(_v - b._v);
        public NumberValue Mul(NumberValue b) => new(_v * b._v);
        public NumberValue Div(NumberValue b)
        { if (b._v == 0) throw new DivideByZeroException(); return new(_v / b._v); }

        public override string ToString() => _v.ToString();
    }
}
