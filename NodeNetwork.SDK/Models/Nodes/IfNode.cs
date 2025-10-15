using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NodeNetworkSDK.Models.Nodes
{
    public sealed class IfNode : INode
    {
        public string Name { get; }
        private readonly string _condKey, _thenKey, _elseKey, _out;

        public IfNode(string name, string condKey, string thenKey, string elseKey, string outKey)
        { 
            Name = name; 
            _condKey = condKey; 
            _thenKey = thenKey; 
            _elseKey = elseKey; 
            _out = outKey; }

        public IContext Exec(IContext ctx)
        {
            bool cond = ctx.Get<bool>(_condKey);
            object val = cond ? ctx.Get<object>(_thenKey) : ctx.Get<object>(_elseKey);
            return ctx.Set(_out, val);
        }
    }
}
