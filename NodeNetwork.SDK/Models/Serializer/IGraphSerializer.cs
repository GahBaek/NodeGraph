using NodeNetworkSDK.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Serializer
{
    public interface IGraphSerializer
    {
        string Serialize(GraphId gid);
        GraphId Deserialize(string json);
        int SchemaVersion { get; }
    }
}
