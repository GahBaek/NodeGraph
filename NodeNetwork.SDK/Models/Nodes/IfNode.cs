using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;

namespace NodeNetworkSDK.Models.Nodes
{
    internal sealed class IfNode : NodeBase
    {
        private static readonly NodeMeta _meta = new(
            "If",
            ins: new[]
            {
                new ParamSpec("cond", BoolType.Instance),
                new ParamSpec("then", NumberType.Instance),
                new ParamSpec("else", NumberType.Instance)
            },
            outs: new[]
            {
                new ParamSpec("out", NumberType.Instance, required: false)
            }
        );

        public IfNode(string name) : base(name, _meta) { }

        public override IReadOnlyDictionary<string, IValue> Execute(IReadOnlyDictionary<string, IValue> inputs)
        {
            var cond = Need<BoolValue>(inputs, "cond");
            var thenVal = Need<NumberValue>(inputs, "then");
            var elseVal = Need<NumberValue>(inputs, "else");

            var result = cond.Value ? thenVal : elseVal;
            return new Dictionary<string, IValue> { ["out"] = result };
        }
    }
}
