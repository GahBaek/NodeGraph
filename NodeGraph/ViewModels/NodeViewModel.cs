using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvMVVM2.Core.MVVM;
using NodeNetworkSDK.Models;
using ConvMVVM2.WPF.ViewModels;
using NodeNetworkSDK.Models.Nodes;
using System.Collections.ObjectModel;

namespace NodeGraph.ViewModels
{
    partial class NodeViewModel : ViewModelBase
    {
        public Guid Id { get; }
        public string Name { get; }
        public NodeMeta Meta { get; }
        public ObservableCollection<PortViewModel> Inputs { get; }
        public ObservableCollection<PortViewModel> Outputs { get; }
        public NodeViewModel(Guid nodeGuid, string nodeId, string instanceName)
        {
            Id = nodeGuid;
            Name = nodeId;
        }

        public void SaveAll()
        {

        }

    }
}
