using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    // data / 실행 상태
    public interface IContext
    {
        void SetInput(NodeHandle node, string inputName, IValue value);
        public void SetOutput(Guid id, string outputName, IValue value);
        void ClearInput(NodeHandle node, string inputName);
        bool TryGetInput(NodeHandle node, string inputName, out IValue value);
        bool TryGetOutput(NodeHandle node, string outputName, out IValue value);
        void ClearAll();
    }
}
