using NodeNetworkSDK.Models;
using NodeNetworkSDK.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeNetwork.SDK.Models
{
    public interface IPage
    {
        string Name { get; }
        string ResultKey { get; }
        INode AddNode(INode node);
        IContext Exec(IContext ctx);
        // IEnumerable<INode> EnumerateNodes();

        bool Connect(Guid fromNodeId, string fromPortKey, Guid toNodeId, string toPortKey);
        bool Disconnect(Guid fromNodeId, string fromPortKey, Guid toNodeId, string toPortKey);
    }
}