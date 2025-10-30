using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Values
{
    public sealed class StringType : IDataType
    {
        public static readonly StringType Instance = new();
        private StringType() { }
        public string Id => "core.string";
        public bool IsAssignableFrom(IDataType other) => ReferenceEquals(this, other);
    }

    public sealed class StringValue : IValue
    {
        public IDataType Type => StringType.Instance;
        public string V { get; }
        public StringValue(string v) { V = v ?? ""; }

        public override string ToString() => V;

        public StringValue Concat(StringValue b) => new(V + b.V);
        public StringValue Substring(int start, int? len = null)
            => new(len is null ? V.Substring(start) : V.Substring(start, len.Value));
        public bool Contains(StringValue sub, StringComparison cmp)
            => V.IndexOf(sub.V, cmp) >= 0;
    }

}
