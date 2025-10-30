using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeNetworkSDK.Models;
using NodeNetworkSDK.Models.Nodes;

namespace NodeGraph.Services
{
    public class NodeRegistry
    {
        public sealed record Entry(string Id, NodeMeta Meta, Func<INode> Factory);
        private static readonly Dictionary<string, Entry> _entries = new();

        public void Register(string id, NodeMeta meta, Func<INode> factory)
        {
            _entries[id] = new Entry(id, meta, factory);
        }

        public bool TryGet(string id, out Entry entry)
        {
            return _entries.TryGetValue(id, out entry);
        }

        public IEnumerable<Entry> All()
        {
            return _entries.Values;
        }

        public static INode Create(string id)
        {
            try {
                _entries.TryGetValue(id, out var e);
                return e.Factory();
            }
            catch
            {
                throw new KeyNotFoundException(id);
            }
        }
    }

}
