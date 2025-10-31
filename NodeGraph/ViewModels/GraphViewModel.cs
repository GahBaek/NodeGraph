using ConvMVVM2.Core.Attributes;
using ConvMVVM2.Core.MVVM;
using Microsoft.Win32;
using NodeGraph.Services;
using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Services;
using NodeNetworkSDK.Services.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace NodeGraph.ViewModels
{
    partial class GraphViewModel : ViewModelBase
    {
        #region private
        private readonly GraphManager _graphManager;
        private readonly GraphSerializer _graphSerializer;
        private readonly GraphId _graphId;
        private readonly Context _context;
        private readonly NodeRegistry _nodeRegistry;
        #endregion

        #region constructor
        public GraphViewModel(GraphManager graphManager, NodeRegistry nodeRegistry, GraphId id, GraphSerializer graphSerializer)
        {
            _graphId = id;
            _graphManager = graphManager;
            _graphSerializer = graphSerializer;
            _nodeRegistry = nodeRegistry;
            _context = new Context();

            // NodeRegistry 에 저장한 Entries 를 NodeTypes에 저장해준다.
            foreach (var n in _nodeRegistry.All())
                NodeTypes.Add(n.Meta);
        }
        #endregion

        #region ObservableCollection
        // BootStrapper 에서 저장한 모든 Nodes 를 갖고 있다.
        public ObservableCollection<NodeMeta> NodeTypes { get; } = new();
        public ObservableCollection<NodeViewModel> Nodes { get; } = new();
        public ObservableCollection<PortViewModel> Edges { get; } = new();
        #endregion

        // 어떤 Node 인지
        public static string GetInstanceName(NodeMeta meta) 
        {
            return meta.Display;
        }

        #region Commands

        [RelayCommand]
        public void AddNode(string nodeId)
        {
            if (!_nodeRegistry.TryGet(nodeId, out var entry))
                throw new KeyNotFoundException(nodeId);

            // 추가하려는 Node 의 이름을 반환 받는다.
            var instanceName = GetInstanceName(entry.Meta);

            var nodeGuid = _graphManager.AddNode(_graphId, nodeId, instanceName);

            var nvm = new NodeViewModel(nodeGuid.Value, nodeId, instanceName);
            foreach (var p in entry.Meta.Inputs)
                nvm.Inputs.Add(new PortViewModel(nodeGuid.Value, p, isInput: true));
            foreach (var p in entry.Meta.Outputs)
                nvm.Outputs.Add(new PortViewModel(nodeGuid.Value, p, isInput: false));

            Nodes.Add(nvm);
        }

        [RelayCommand]
        public void Connect(PortSelection sel)
        {
            if (sel == null || !sel.IsValid)
                return;

            _graphManager.Connect(_graphId, new NodeHandle(sel.FromNode), 
                sel.FromPort, new NodeHandle(sel.ToNode), sel.ToPort);

            Edges.Add(new PortViewModel(sel.FromNode, sel.FromPort, sel.ToNode, sel.ToPort));
        }

        [RelayCommand]
        public void Disconnect(PortSelection sel)
        {
            if (sel == null || !sel.IsValid)
                return;

            _graphManager.Disconnect(_graphId,new NodeHandle(sel.FromNode), sel.FromPort, new NodeHandle(sel.ToNode), sel.ToPort);

            // Edges 에서 해당 Edge 찾기
            var edge = Edges.FirstOrDefault(e =>
                e.FromNode == sel.FromNode && e.FromPort == sel.FromPort &&
                e.ToNode == sel.ToNode && e.ToPort == sel.ToPort);

            // 삭제
            if (edge != null) 
                Edges.Remove(edge);
        }

        [RelayCommand]
        public void Execute()
        {
            // 순환이 있는 지 검사.
            var (ok, errs) = _graphManager.Validate(_graphId, _context);

            if (!ok)
            {
                return;
            }

            _graphManager.Execute(_graphId, _context);
        }

        [RelayCommand]
        public void Save()
        {
            var dialog = new SaveFileDialog();
            dialog.Title = "그래프 저장";
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            dialog.FileName = "graph.json";
            dialog.AddExtension = true;
            dialog.DefaultExt = ".json";
            
            if (dialog.ShowDialog() != true)
                return;

            string json = _graphSerializer.Serialize(_graphId);
            File.WriteAllText(dialog.FileName, json); 
        }

        [RelayCommand]
        public void Load()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "그래프 불러오기";
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() != true) return;

            string json = File.ReadAllText(dialog.FileName);
            GraphId newId = _graphSerializer.Deserialize(json);

            RebuildViewFromGraph(newId);
        }
        #endregion

        #region private method
        // Json DeSerialization -> Graph
        private void RebuildViewFromGraph(GraphId id)
        {
            Nodes.Clear();
            Edges.Clear();

            // 해당 Id 를 만족하는 모든 Node 와 Edge 를 반환 받는다.
            var nodes = _graphManager.ListNodes(id);
            foreach (var (nodeId, nodeType, display) in nodes)
            {
                if (!_nodeRegistry.TryGet(nodeType, out var entry))
                    continue;

                var nvm = new NodeViewModel(nodeId, nodeType, display);

                foreach (var p in entry.Meta.Inputs)
                    nvm.Inputs.Add(new PortViewModel(nodeId, p, isInput: true)); 
                foreach (var p in entry.Meta.Outputs)
                    nvm.Outputs.Add(new PortViewModel(nodeId, p, isInput: false));

                Nodes.Add(nvm);
            }

            var edges = _graphManager.ListEdges(id);
            foreach (var (from, outPort, to, inPort) in edges)
                Edges.Add(new PortViewModel(from, outPort, to, inPort));
        }
        #endregion
    }
}
