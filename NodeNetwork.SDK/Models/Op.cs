using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    public sealed class Op : INode
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public IReadOnlyList<string> Inputs { get; }
        public IReadOnlyList<string> Outputs { get; }

        private readonly Func<double, double, double> _op;
        private readonly string _aKey, _bKey, _outKey;

        public Op(string name, string aKey, string bKey, string outKey, Func<double, double, double> op)
        {
            Name = name;
            _aKey = aKey; _bKey = bKey; _outKey = outKey;
            _op = op;
            Inputs = new[] { _aKey, _bKey };
            Outputs = new[] { _outKey };
        }

        public IContext Exec(IContext ctx)
        {
            if (!ctx.TryGet<double>(_aKey, out var a))
                throw new InvalidOperationException($"[{Name}] missing '{_aKey}'");
            if (!ctx.TryGet<double>(_bKey, out var b))
                throw new InvalidOperationException($"[{Name}] missing '{_bKey}'");

            var res = _op(a, b);
            return ctx.With(_outKey, res);
        }
    }
}
