using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvMVVM2.Core.MVVM;
using NodeNetworkSDK.Models;
using ConvMVVM2.WPF.ViewModels;
using NodeNetworkSDK.Models.Nodes;

namespace NodeGraph.ViewModels
{
    partial class NodeViewModel : ViewModelBase
    {
        public Guid Id { get; }
        public string Name { get; }
        public NodeMeta Meta { get; }
    }
}
