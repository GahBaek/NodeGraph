using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    // 연산 로직
    public interface INode
    {
        string Name { get; }
        IContext Exec(IContext ctx);
    }
}
