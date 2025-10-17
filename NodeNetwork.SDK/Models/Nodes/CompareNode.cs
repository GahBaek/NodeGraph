using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public enum Comparator { Lt, Le, Gt, Ge, Eq, Ne }
    public sealed class CompareNode : INode
    {
        public string Name { get; }
        public Guid Id { get; }

        private static readonly IReadOnlyDictionary<string, Port> _inputSpec =
            new Dictionary<string, Port>
            {
            };

        private static readonly IReadOnlyDictionary<string, Port> _outputSpec =
            new Dictionary<string, Port>
            {
                ["out"] = new Port(typeof(object))
            };

        public IReadOnlyDictionary<string, Port> Inputs => _inputSpec;
        public IReadOnlyDictionary<string, Port> Outputs => _outputSpec;

        private readonly string _a, _b, _out;
        private readonly Comparator _cmp;

        public CompareNode(string name, string aKey, string bKey, string outKey, Comparator cmp)
        { Name = name; _a = aKey; _b = bKey; _out = outKey; _cmp = cmp; Id = Guid.NewGuid();  }

        public IContext Exec(IContext ctx)
        {
            double a = ctx.Get<double>(_a);
            double b = ctx.Get<double>(_b);
            bool r = _cmp switch
            {
                Comparator.Lt => a <  b,
                Comparator.Le => a <= b,
                Comparator.Gt => a >  b,
                Comparator.Ge => a >= b,
                Comparator.Eq => Math.Abs(a - b) < 1e-9,
                Comparator.Ne => Math.Abs(a - b) >= 1e-9,
                _ => throw new NotSupportedException(_cmp.ToString())
            };
            return ctx.Set(_out, r);
        }

        public INode CloneWithKeyRemap(Func<string, string> remap)
        {
            return new CompareNode(Name, remap(_a), remap(_b), remap(_out), _cmp);
        }
    }
}
