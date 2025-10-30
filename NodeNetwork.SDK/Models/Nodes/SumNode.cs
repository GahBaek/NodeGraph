using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public class SumNode : NodeBase
    {
        // Metadata
        public static readonly NodeMeta _meta = new(
            "Sum",
            ins: new[] { new ParamSpec("a", NumberType.Instance), new ParamSpec("b", NumberType.Instance) },
            outs: new[] { new ParamSpec("out", NumberType.Instance, required: false) }
        );
        public SumNode(string name) : base(name, _meta) { }
        public SumNode() : base(_meta.Display, _meta) { }

        public override IReadOnlyDictionary<string, IValue> Execute(IReadOnlyDictionary<string, IValue> inputs)
        {
            var a = Need<NumberValue>(inputs, "a"); var b = Need<NumberValue>(inputs, "b");
            return new Dictionary<string, IValue> { ["out"] = a.Add(b) };
        }
    }
}
