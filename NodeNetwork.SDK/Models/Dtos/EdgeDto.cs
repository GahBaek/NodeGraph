using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Dtos
{
    public class EdgeDto
    {
        public Guid From { get; set; }
        public string Out { get; set; } = "";
        public Guid To { get; set; }
        public string In { get; set; } = "";
    }
}
