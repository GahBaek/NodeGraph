using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    // true/false 에 따라 이미 계산되어 있는 두 값 중 하나를 고르는 노드
    public class SelectNode<T> : INode
    {
        public string Cond { get; }
        public string ThenKey { get; }
        public string ElseKey { get; }
        public string Out { get; }
        public string Name { get; }
        public IReadOnlyCollection<string> Inputs => new[] { Cond, ThenKey, ElseKey };
        public IReadOnlyCollection<string> Outputs => new[] { Out };

        public SelectNode(string name, string cond, string thenKey, string elseKey, string @out)
        { Name = name; Cond = cond; ThenKey = thenKey; ElseKey = elseKey; Out = @out; }

        public IContext Exec(IContext ctx)
        {
            bool c = ctx.Get<bool>(Cond);
            var v = ctx.Get<T>(c ? ThenKey : ElseKey);
            return ctx.Set(Out, v);
        }
    }
}
