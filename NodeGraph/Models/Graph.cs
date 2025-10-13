using Microsoft.CodeAnalysis.CSharp.Syntax;
using NodeGraph.Models.Dtos;
using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.Models
{
    // 그래프 단위
    class Graph
    {
        #region Public Property
        public Page Page { get; }                  
        public string? FilePath { get; set; }
        public bool IsDirty { get; set; } // save
        private List<NodeModel> Nodes = new List<NodeModel>();
        private List<EdgeModel> Edges = new List<EdgeModel>();
        
        public Graph(Page page) => Page = page;
        #endregion // Public Property

        #region Public Functions
        public void UpdateNodes(List<NodeModel> nodes)
        {
            this.Nodes = nodes;
        }
        public void UpdateEdges(List<EdgeModel> edges)
        {
            this.Edges = edges;
        }
        #endregion // Public Functions

        #region Converter
        public GraphDto ToDto()
        {
            return new GraphDto()
            {
                Name = Page.Name,
                ResultKey = Page.ResultKey,
                Nodes = Nodes.Select(x => x.ToDto()).ToList(),
                Edges = Edges.Select(x => x.ToDto()).ToList()
            };
        }

        public static Graph Convert(GraphDto dto, Page page)
        {
            // page 수정해야함.
            var graph = new Graph(page)
            {
                
            };
            var nodes = dto.Nodes.Select(dto => NodeModel.Converter(dto)).ToList();
            var edges = dto.Edges.Select(dto => EdgeModel.Converter(dto)).ToList();

            graph.UpdateNodes(nodes);
            graph.UpdateEdges(edges);

            return graph;
        }
        #endregion // Converter
    }
}
