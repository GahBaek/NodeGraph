using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    public interface IEdge
    {
        Guid FromNodeId { get; }
        string FromPortKey { get; }
        Guid ToNodeId { get; }
        string ToPortKey { get; }
    }
}
