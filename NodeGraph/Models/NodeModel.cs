using NodeGraph.Models.Dtos;
using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NodeGraph.Models
{
    public class NodeModel
    {
        #region Public Property
        public Guid NodeId { get; set; }
        public INode Node { get; set; }
        // Node 시각화
        public double x;
        public double y;
        public double width = 100;
        public double height = 100;
        #endregion // Public Property

        #region Public Functions
        public void UpdateNode(INode node)
        {
            this.Node = node;
        }
        public void UpdateNodeId(Guid id)
        {
            this.NodeId = id;
        }
        #endregion // Public Functions

        #region Converter
        public NodeDto ToDto()
        {
            return new NodeDto
            {
                x = x,
                y = y,
                width = width,
                height = height
            };
        }
        public static NodeModel Converter(NodeDto dto)
        {
            var node = new NodeModel
            {
                x = dto.x,
                y = dto.y,
                width = dto.width,
                height = dto.height
            };
            return node;
        }
        #endregion // Converter
    }
}
