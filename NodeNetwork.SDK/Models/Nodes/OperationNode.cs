using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public enum Operation { Add, Sub, Mul, Div }

    public sealed class OperationNode : INode
    {
        public string Name { get; }
        private readonly string _a, _b, _out;
        private readonly Operation _op;

        public OperationNode(string name, string aKey, string bKey, string outKey, Operation op)
        { Name = name; _a = aKey; _b = bKey; _out = outKey; _op = op; }


        public IContext Exec(IContext ctx)
        {
            double a = ctx.Get<double>(_a);
            double b = ctx.Get<double>(_b);
            double r = _op switch
            {
                Operation.Add => a + b,
                Operation.Sub => a - b,
                Operation.Mul => a * b,
                Operation.Div => b == 0 ? throw new DivideByZeroException(Name) : a / b,
                _ => throw new NotSupportedException(_op.ToString())
            };
            return ctx.Set(_out, r);
        }
    }
}
