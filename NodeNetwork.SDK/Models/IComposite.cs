using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public interface IComposite :INode
    {
        
    }
    public class CompositeNode : IComposite
    {

        private readonly List<INode> children;

        public Guid Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public IReadOnlyList<string> Inputs => throw new NotImplementedException();

        public IReadOnlyList<string> Outputs => throw new NotImplementedException();

        public INode AddNode(INode child)
        {
            if (ReferenceEquals(child, this))
                throw new InvalidOperationException("자기 자신을 추가할 수 없습니다.");


            children.Add(child);
            return child;
        }

        public IContext Exec(IContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
