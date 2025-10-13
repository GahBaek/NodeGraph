using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    public interface INode
    {
        Guid Id { get; }
        string Name { get; }           
        IReadOnlyList<string> Inputs { get; } 
        IReadOnlyList<string> Outputs { get; } 

        IContext Exec(IContext ctx);
    }
}
