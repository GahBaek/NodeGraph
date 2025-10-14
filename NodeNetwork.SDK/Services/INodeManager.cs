using NodeNetwork.SDK.Models;
using NodeNetworkSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Services
{

    public interface INodeManager
    {
        // var page = manager.createNode(context, "Page", "Page1", "Page.a");
        IComposite CreateNode(IContext context, string contextType, string contextName /*inputs*/);
        // var ifelse = manager.addNode(page, IContext, "IfElse", "IfElse1", "sectionName");
        IContext AddNode(IComposite context, string contextType, string contextName /*inputs*/);
        void RemoveNode();
    }

    public class NodeManager : INodeManager
    {
        public string key;
        public string contextType;
        public NodeManager(string key) { }
        public IContext AddNode(IComposite context, string contextType, string contextName)
        {
            throw new NotImplementedException();
        }

        public IComposite CreateNode(IContext context, string contextType, string contextName)
        {
            throw new NotImplementedException();
        }

        public void RemoveNode()
        {
            throw new NotImplementedException();
        }
    }
}
