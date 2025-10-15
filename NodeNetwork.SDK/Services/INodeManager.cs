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
    public interface INodeManager
    {
        void Add(IPage page, INode node);

        OperationNode Operation(string name, string aKey, string bKey, string outKey, Operation op);
        CompareNode Compare(string name, string aKey, string bKey, string outKey, Comparator cmp);
        IfNode If(string name, string condKey, string thenKey, string elseKey, string outKey);
        SelectNode Select(string name, string selectorKey, string outKey, params (string caseValue, string fromKey)[] routes);
    }
}
