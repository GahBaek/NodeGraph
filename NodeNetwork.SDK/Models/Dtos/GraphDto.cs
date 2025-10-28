using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Dtos
{
    public  class GraphDto
    {
        public int SchemaVersion { get; set; } = 1;
        public string Name { get; set; } = "";
        public List<NodeDto> Nodes { get; set; } = new();
        public List<EdgeDto> Edges { get; set; } = new();
        public List<LiteralDto> Literals { get; set; } = new();
    }
}
