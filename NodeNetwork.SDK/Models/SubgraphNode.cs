using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public class SubgraphNode : INode
    {
        public IPage Graph { get; }
        public IReadOnlyDictionary<string, string> InMap { get; }   // callerKey -> calleeKey
        public IReadOnlyDictionary<string, string> OutMap { get; }  // calleeKey -> callerKey
        public string Name { get; }
        public IReadOnlyCollection<string> Inputs => InMap.Keys.ToArray();
        public IReadOnlyCollection<string> Outputs => OutMap.Values.ToArray();

        public SubgraphNode(string name, IPage graph,
            IReadOnlyDictionary<string, string> inMap,
            IReadOnlyDictionary<string, string> outMap)
        { Name = name; Graph = graph; InMap = inMap; OutMap = outMap; }

        public IContext Exec(IContext ctx)
        {
            var sub = ctx.SpawnScoped();
            foreach (var (src, dst) in InMap) sub.Set(dst, ctx.Get<object>(src));

            var res = Graph.Exec(sub);

            foreach (var (src, dst) in OutMap) ctx.Set(dst, res.Get<object>(src));
            return ctx;
        }
    }
}
