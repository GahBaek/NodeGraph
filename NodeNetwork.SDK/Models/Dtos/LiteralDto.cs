using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Dtos
{
    public class LiteralDto
    {
        public Guid Node { get; set; }
        public string Input { get; set; } = "";
        public string TypeId { get; set; } = "";
        public object? Payload { get; set; }
    }
}
