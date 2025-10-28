using ConvMVVM2.Core.MVVM;
using ConvMVVM2.WPF.ViewModels;
using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    partial class PortViewModel : ViewModelBase
    {
        public string Name { get; }
        public bool HasEdge { get; set; }
        public IValue? Literal { get; set; }
    }
}

