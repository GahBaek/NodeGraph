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
        public bool IsValid
        {
            get
            {
                if (FromNode == Guid.Empty)
                    return false;

                if (ToNode == Guid.Empty)
                    return false;

                if (string.IsNullOrWhiteSpace(FromPort))
                    return false;

                if (string.IsNullOrWhiteSpace(ToPort))
                    return false;

                return true;
            }
        }

    }
}
