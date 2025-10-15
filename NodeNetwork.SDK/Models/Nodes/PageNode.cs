using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    // page 를 가질 수 있다.
    public class PageNode : INode
    {
        public string Name { get; }
        private readonly IPage _child;
        private readonly string _outKey;

        // child.ResultKey에서 읽어 부모의 outKey로 복사
        public PageNode(string name, IPage child, string outKey)
        {
            Name = name;
            _child = child;
            _outKey = outKey;
        }

        public IContext Exec(IContext ctx)
        {
            ctx = _child.Exec(ctx);

            // 자식 페이지의 결과를 부모 outKey로 복사
            var v = ctx.Get<object>(_child.ResultKey);
            return ctx.Set(_outKey, v);
        }

        /* public IContext Exec(IContext ctx)
         {
             var sub = ctx.SpawnScoped();
             foreach (var (src, dst) in InMap) sub.Set(dst, ctx.Get<object>(src));

             var res = Graph.Exec(sub);

             foreach (var (src, dst) in OutMap) ctx.Set(dst, res.Get<object>(src));
             return ctx;
         }*/
    }
}
