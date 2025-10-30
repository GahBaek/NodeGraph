using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public class MulNode : NodeBase
    {
        // Metadata
        public static readonly NodeMeta _meta = new(
            "Mul",
            new[] {new ParamSpec("a", NumberType.Instance), new ParamSpec("b", NumberType.Instance) },
            new[] { new ParamSpec("out", NumberType.Instance, required :false) }
            );

        public MulNode(string name) : base(name, _meta) { }
        public MulNode() : base(_meta.Display, _meta) { }

        public override IReadOnlyDictionary<string, IValue> Execute(IReadOnlyDictionary<string, IValue> inputs) {
            var a = Need<NumberValue>(inputs, "a");
            var b = Need<NumberValue>(inputs, "b");
            return new Dictionary<string, IValue> { ["out"] = a.Mul(b) };
        }


    }
}
