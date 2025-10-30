using ConvMVVM2.Core.MVVM;
using ConvMVVM2.WPF.ViewModels;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    public partial class PortViewModel : ViewModelBase
    {
        public Guid NodeId { get; }
        public ParamSpec Spec { get; }
        public bool IsInput { get; }

        public PortViewModel(Guid nodeId, ParamSpec spec, bool isInput)
        {
            NodeId = nodeId;
            Spec = spec;
            IsInput = isInput;
        }

        public Guid FromNode { get; }
        public string FromPort { get; }
        public Guid ToNode { get; }
        public string ToPort { get; }

        public PortViewModel(Guid fromNode, string fromPort, Guid toNode, string toPort)
        {
            FromNode = fromNode;
            FromPort = fromPort;
            ToNode = toNode;
            ToPort = toPort;
        }
    }

}

