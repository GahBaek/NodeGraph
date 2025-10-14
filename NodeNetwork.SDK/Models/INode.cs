using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    // 연산 조직
    public interface INode
    {
        IReadOnlyCollection<string> Inputs { get; }  
        IReadOnlyCollection<string> Outputs { get; } 
        IContext Exec(IContext ctx);                 
        string Name { get; }
    }
}
