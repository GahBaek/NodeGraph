using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    public sealed record Edge(
    Guid Id,
    Guid FromNodeId,
    string FromPortKey,
    Guid ToNodeId,
    string ToPortKey
);

}
