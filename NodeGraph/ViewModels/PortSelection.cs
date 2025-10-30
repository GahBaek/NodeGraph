using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    public sealed class PortSelection
    {
        public Guid FromNode { get; init; }
        public string FromPort { get; init; }
        public Guid ToNode { get; init; }
        public string ToPort { get; init; }
        public bool IsValid => FromNode != Guid.Empty && ToNode != Guid.Empty &&
                               !string.IsNullOrWhiteSpace(FromPort) && !string.IsNullOrWhiteSpace(ToPort);
    }
}
