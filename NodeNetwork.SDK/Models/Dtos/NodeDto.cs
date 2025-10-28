using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Dtos
{
    public class NodeDto
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = ""; // 풀네임 권장
        public string Name { get; set; } = "";
    }
}
