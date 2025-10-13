using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.Models.Dtos
{
    public class EdgeDto
    {
        #region Public Property
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        // edge시각화
        public double startX { get; set; } = 0;
        public double startY { get; set; } = 0;
        public double endX { get; set; } = 0;
        public double endY { get; set; } = 0;
        #endregion // Public Property
    }
}
