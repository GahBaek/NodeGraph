using NodeGraph.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.Models
{
    public class EdgeModel
    {
        #region Public Property
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        // edge시각화
        public double startX;
        public double startY;
        public double endX;
        public double endY;
        #endregion // Public Property

        #region Converter
        public EdgeDto ToDto()
        {
            return new EdgeDto()
            {
                startX = startX,
                startY = startY,
                endX = endX,
                endY = endY
            };
        }
        public static EdgeModel Converter(EdgeDto dto)
        {
            var edge = new EdgeModel()
            {
                startX = dto.startX,
                startY = dto.startY,
                endX = dto.endX,
                endY = dto.endY
            };
            return edge;
        }
        #endregion // Converter
    }
}
