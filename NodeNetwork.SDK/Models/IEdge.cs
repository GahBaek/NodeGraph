using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    public interface IEdge
    {
        Guid Id { get; }
        string Name { get; }              // 표시용(로깅/디버깅), 식별은 Guid로
        IReadOnlyList<string> Inputs { get; }   // 컨텍스트 키(논리 포트)
        IReadOnlyList<string> Outputs { get; }  // 컨텍스트 키(논리 포트)

        IContext Exec(IContext ctx);
    }
}
