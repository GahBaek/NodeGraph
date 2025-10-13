using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.Models.Dtos
{
    public class GraphDto
    {
        #region Public Property
        public string? Name { get; set; }
        public string? ResultKey { get; set; }
        public List<NodeDto> Nodes { get; set; }
        public List<EdgeDto> Edges { get; set; }
        #endregion
    }
}
