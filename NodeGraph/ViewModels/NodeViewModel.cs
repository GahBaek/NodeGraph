using ConvMVVM2.Core.MVVM;
using NodeGraph.Models;
using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    public partial class NodeViewModel : ViewModelBase
    {
        #region Constructor
        public NodeViewModel() { }
        #endregion // Constructor

        #region Public Property
        public NodeModel NodeModel { get; set; }
        public Guid Id => Node.Id;
        public string Name => Node.Name;
        public INode Node { get; }
        public IEnumerable<string> InputKeys => Node.Inputs;
        public IEnumerable<string> OutputKeys => Node.Outputs;
        #endregion // Public Property

        #region Public Function
        #endregion // Public Function

        #region Command
        #endregion // Command
    }
}
