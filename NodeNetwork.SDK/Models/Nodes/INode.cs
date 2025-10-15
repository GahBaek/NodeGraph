using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    // 연산 로직
    public interface INode
    {
        string Name { get; }
        IContext Exec(IContext ctx);
    }
}
