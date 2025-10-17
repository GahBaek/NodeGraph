using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    // 연산 로직
    public interface INode
    {
        Guid Id { get; }
        string Name { get; }
        private static readonly IReadOnlyDictionary<string, Port> _inputSpec;

        private static readonly IReadOnlyDictionary<string, Port> _outputSpec;
        IContext Exec(IContext ctx);
    }
}
