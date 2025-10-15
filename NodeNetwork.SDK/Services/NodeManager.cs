using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models;
using NodeNetworkSDK.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Services
{
    public class NodeManager : INodeManager
    {
        // 실질적인 addNode
        public void Add(IPage page, INode node)
        {
            page.AddNode(node);
        }

        // 각 유형에 맞는 INode return 하는 함수들
        public OperationNode Operation(string name, string aKey, string bKey, string outKey, Operation op)
        {
            return new(name, aKey, bKey, outKey, op);
        }
        public CompareNode Compare(string name, string aKey, string bKey, string outKey, Comparator cmp)
        {
            return new(name, aKey, bKey, outKey, cmp);
        }
        public IfNode If(string name, string condKey, string thenKey, string elseKey, string outKey)
        {
            return new(name, condKey, thenKey, elseKey, outKey);
        }
        public SelectNode Select(string name, string selectorKey, string outKey, params (string, string)[] routes)
        {
            return new(name, selectorKey, outKey, routes);
        }
    }
}
