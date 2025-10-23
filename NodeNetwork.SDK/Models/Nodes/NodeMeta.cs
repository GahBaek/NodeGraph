using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    /*
     * 정적 메타데이터(스키마)
     * 화면에 어떻게 보이고, 어떤 I/O 를 어떤 이름, 타입으로 가진다(= ParamSpec)는 것을 기술
     */
    public class NodeMeta
    {
        public string Display { get; }
        public IReadOnlyList<ParamSpec> Inputs { get; }
        public IReadOnlyList<ParamSpec> Outputs { get; }
        public NodeMeta(string display, IReadOnlyList<ParamSpec> ins, IReadOnlyList<ParamSpec> outs)
        { Display = display; Inputs = ins; Outputs = outs; }
    }
}
