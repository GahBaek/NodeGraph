using ConvMVVM2.Core.Attributes;
using ConvMVVM2.Core.MVVM;
using DevExpress.Data.Async;
using NodeGraph.Models;
using NodeNetwork.SDK.Models;
using NodeNetwork.SDK.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    public partial class PageViewModel : ViewModelBase
    {
        #region Constructor
        public PageViewModel(Page page, IPageManager pageManager, Func<NodeModel, NodeViewModel> NodeFactory, 
            Func<EdgeModel, EdgeViewModel> EdgeFactory) {
            this.pageModel = page;
            this.pageManager = pageManager;
            this.NodeFactory = NodeFactory;
            this.EdgeFactory = EdgeFactory;

            pageManager.RegisterPage(pageModel);
            ctx = Ctx.Empty();
        }
        #endregion // Constructor

        #region Private Property
        private readonly IPageManager pageManager;
        private readonly Func<NodeModel, NodeViewModel> NodeFactory;
        private readonly Func<EdgeModel, EdgeViewModel> EdgeFactory;
        private IContext ctx;
        #endregion // Private Property

        #region Public Property
        public Page pageModel;

        public ObservableCollection<NodeViewModel> Nodes { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }
        
        [Property]
        public string Name;
        [Property]
        public string ResultKey;

        [Property]
        public string OpName;
        [Property]
        public string Input1;
        [Property]
        public string Input2;
        [Property]
        public string ResultName;
        
        public Func<double, double, double> Operation { get; set; } = Operations.Add;

        [Property]
        public NodeViewModel SelectedNodeFrom;
        [Property]
        public NodeViewModel SelectedNodeTo;
        [Property]
        public string fromKey = "";
        [Property]
        public string toKey = "";
        [Property]
        public EdgeViewModel SelectedEdgeVM;
        #endregion // Public Property

        #region Command
        // Node 추가
        [RelayCommand]
        public void AddNode()
        {
            if (SelectedNodeFrom != null)
                return;

            INode node = new Op(OpName, Input1, Input2, ResultName, Operation);
            var newNodeId = pageModel.AddNode(node);

            // ????
            ctx.With(OpName, ResultName);

            var nodeModel = new NodeModel();
            nodeModel.UpdateNode(node);

            var nodevm = this.NodeFactory(nodeModel);
            this.Nodes.Add(nodevm);
            SelectedNodeFrom = nodevm;
        }

        //Node 삭제
        [RelayCommand]
        public void RemoveNode()
        {
            if (this.SelectedNodeFrom == null)
                return;
            Guid selectedNodeId = this.SelectedNodeFrom.Id;
            try
            {
                pageModel.RemoveNode(selectedNodeId);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("RemoveNode(): "+ex.ToString());
            }
        }

        // Edge 추가
        [RelayCommand]
        public void AddEdge()
        {
            if(SelectedEdgeVM != null) return;

            var edgeModel = new EdgeModel();
            var edgeVM = EdgeFactory(edgeModel);
            this.Edges.Add(edgeVM);
            SelectedEdgeVM = edgeVM;
        }

        // Edge 와 Node 연결
        [RelayCommand]
        public void ConnectNode()
        {
            if (this.SelectedNodeFrom == null || this.SelectedNodeTo == null)
                return;
            if (this.fromKey == null || this.toKey == null)
                return;
            try
            { 
                pageModel.Connect(SelectedNodeFrom.Id, fromKey, SelectedNodeFrom.Id, toKey); 
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ConnectNode: " + e.ToString());
            }
        }

        // Edge 삭제
        [RelayCommand]
        public void RemoveEdge()
        {
            if (this.SelectedNodeFrom == null || this.SelectedNodeTo == null)
                return;
            if (this.fromKey == null || this.toKey == null)
                return;
            try
            {
                pageModel.Disconnect(SelectedNodeFrom.Id, fromKey, SelectedNodeFrom.Id, toKey);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("RemoveEdge: " + e.ToString());
            }
        }

        // 실행하기 버튼 클릭
        [RelayCommand]
        public void Execute()
        {
            pageManager.Run(pageModel.Id, ctx);
        }
        #endregion // Command
    }
}
