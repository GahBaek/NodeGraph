using ConvMVVM2.Core.MVVM;
using NodeGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    public partial class EdgeViewModel : ViewModelBase
    {
        #region Constructor
        public EdgeViewModel(NodeViewModel from, string formKey, NodeViewModel to, string toKey)
        {
            From = from;
            FromKey = FromKey;
            To = to;
            ToKey = toKey;

        }
        #endregion // Constructor

        #region Public Property
        public EdgeModel Edge { get; set; }
        public NodeViewModel From { get; }
        public string FromKey { get; }
        public NodeViewModel To { get; }
        public string ToKey { get; }
        #endregion // Public Property

        #region Command
        #endregion // Command
    }
}
