using ConvMVVM2.Core.Attributes;
using ConvMVVM2.Core.MVVM;
using ConvMVVM2.WPF.ViewModels;
using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Services;
using NodeNetworkSDK.Services.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NodeGraph.ViewModels
{
    partial class GraphViewModel : ViewModelBase
    {
        #region private
        private readonly GraphManager _graphManager;
        private readonly GraphSerializer _graphSerializer;
        private readonly GraphId _graphId;
        private readonly Context _context;
        #endregion

        #region constructor
        public GraphViewModel(GraphManager graphManager, GraphId id, GraphSerializer graphSerializer)
        {
            this._graphId = id;
            this._graphManager = graphManager;
            this._graphSerializer = graphSerializer;

        }
        #endregion

        #region ObservableCollection
        public ObservableCollection<NodeViewModel> Nodes { get; } = new();
        public ObservableCollection<PortViewModel> Edges { get; } = new();
        #endregion

        #region relayCommand
        // 노드 추가
        [RelayCommand]
        public void addNode()
        {
            // _graphManager.AddNode();
        }

        // 노드 연결
        [RelayCommand]
        public void Connect()
        {
            // _graphManager.Connect();
        } 

        // 노드 연결 해제
        [RelayCommand]
        public void Disconnect()
        {
            // _graphManager.Disconnect();
        }

        // 실행
        [RelayCommand]
        public void Execute()
        {
            var (ok, errs) = _graphManager.Validate(_graphId, _context);
            if (!ok) return;

            _graphManager.Execute(_graphId, _context);
        }
        [RelayCommand]
        public void Save()
        {
            _graphSerializer.Serialize(_graphId);
        }

        [RelayCommand]
        public void Load()
        {
            // _graphSerializer.Deserialize();
        }
        #endregion

    }
}
