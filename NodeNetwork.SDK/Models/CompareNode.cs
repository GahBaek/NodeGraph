using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public class CompareNode : INode
    {
        public enum Op { GT, GE, EQ, NE, LT, LE }
        public string L { get; }
        public string R { get; }
        public string Out { get; } // bool
        public Op Operation { get; }
        public string Name { get; }
        public IReadOnlyCollection<string> Inputs => new[] { L, R };
        public IReadOnlyCollection<string> Outputs => new[] { Out };

        public CompareNode(string name, string l, string r, string @out, Op op)
        { Name = name; L = l; R = r; Out = @out; Operation = op; }

        public IContext Exec(IContext ctx)
        {
            var a = ctx.Get<double>(L);
            var b = ctx.Get<double>(R);
            bool c = Operation switch
            {
                Op.GT => a > b,
                Op.GE => a >= b,
                Op.EQ => a == b,
                Op.NE => a != b,
                Op.LT => a < b,
                _ => a <= b
            };
            return ctx.Set(Out, c);
        }
    }
}
