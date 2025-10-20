using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public class NodeMeta
    {
        public string Display { get; }
        public IReadOnlyList<ParamSpec> Inputs { get; }
        public IReadOnlyList<ParamSpec> Outputs { get; }
        public NodeMeta(string display, IReadOnlyList<ParamSpec> ins, IReadOnlyList<ParamSpec> outs)
        { Display = display; Inputs = ins; Outputs = outs; }
    }
}
