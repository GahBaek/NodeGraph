using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Dtos
{
    public class SubgraphEntryDto
    {
        public Guid OwnerNodeId { get; set; }  
        public GraphDto Graph { get; set; } = new();
    }
}
