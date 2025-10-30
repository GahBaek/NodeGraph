using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Models.Values;


namespace NodeNetwork.SDK.Models
{
    public class Context : IContext
    {
        internal readonly Dictionary<(Guid Node, string Name), IValue> _inputs = new();
        internal readonly Dictionary<(Guid Node, string Name), IValue> _outputs = new();


        public IEnumerable<(Guid nodeId, string name, IValue value)> DebugInputs()
    => _inputs.Select(kv => (kv.Key.Node, kv.Key.Name, kv.Value));


        public void ClearAll()
        {
            _inputs.Clear();
            _outputs.Clear();
        }

        public void ClearInput(NodeHandle node, string inputName)
        {
            _inputs.Remove((node.Value, inputName));
        }

        public void SetInput(NodeHandle node, string inputName, IValue value)
        {
            _inputs[(node.Value, inputName)] = value;
        }

        public void SetOutput(Guid id, string outputName, IValue value) {
            _outputs[(id, outputName)] = value;
        }

        public bool TryGetInput(NodeHandle node, string inputName, out IValue value)
        {
            return _inputs.TryGetValue((node.Value, inputName), out value!);
        }

        public bool TryGetOutput(NodeHandle node, string outputName, out IValue value)
        {
            return _outputs.TryGetValue((node.Value, outputName), out value!);
        }

        public void Log(string message)
        {
            Console.WriteLine("log: " + message);
        }
    }
}
