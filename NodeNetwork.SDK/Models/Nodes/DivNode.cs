using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public class DivNode : NodeBase
    {
        // Metadata
        public static readonly NodeMeta _meta = new(
        "Div",
        new[] { new ParamSpec("a", NumberType.Instance), new ParamSpec("b", NumberType.Instance) },
        new[] { new ParamSpec("out", NumberType.Instance, required: false) }
    );

        public DivNode(string name) : base(name, _meta) { }
        public DivNode() : base(_meta.Display, _meta) { }

        public override IReadOnlyDictionary<string, IValue> Execute(IReadOnlyDictionary<string, IValue> inputs)
        {
            var a = Need<NumberValue>(inputs, "a"); var b = Need<NumberValue>(inputs, "b");
            return new Dictionary<string, IValue> { ["out"] = a.Div(b) };
        }

    }
}
