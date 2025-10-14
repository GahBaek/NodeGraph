using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public class IfNode : INode
    {
        public string Cond { get; }
        public IPage ThenGraph { get; }
        public IPage ElseGraph { get; }
        public string Name { get; }
        public IReadOnlyCollection<string> Inputs => new[] { Cond }; // 서브그래프 키는 내부에서 사용
        public IReadOnlyCollection<string> Outputs => Array.Empty<string>(); // 결과는 서브그래프가 ctx에 씀

        public IfNode(string name, string cond, IPage thenGraph, IPage elseGraph)
        { Name = name; Cond = cond; ThenGraph = thenGraph; ElseGraph = elseGraph; }

        public IContext Exec(IContext ctx)
        {
            bool c = ctx.Get<bool>(Cond);
            return (c ? ThenGraph : ElseGraph).Exec(ctx);
        }
    }
}
