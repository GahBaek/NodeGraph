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
        public Guid Id { get; }

        private static readonly IReadOnlyDictionary<string, Port> _inputSpec =
            new Dictionary<string, Port>
            {
                ["cond"] = new Port(typeof(bool)),
                ["then"] = new Port(typeof(object)),
                ["else"] = new Port(typeof(object))
            };

        private static readonly IReadOnlyDictionary<string, Port> _outputSpec =
            new Dictionary<string, Port>
            {
                ["out"] = new Port(typeof(object))
            };

        public IReadOnlyDictionary<string, Port> Inputs => _inputSpec;
        public IReadOnlyDictionary<string, Port> Outputs => _outputSpec;
        private readonly string _condKey, _thenKey, _elseKey, _out;

        public IfNode(string name, string condKey, string thenKey, string elseKey, string outKey)
        { 
            Name = name; 
            _condKey = condKey; 
            _thenKey = thenKey; 
            _elseKey = elseKey; 
            _out = outKey; 
            Id = Guid.NewGuid();
        }

        public IContext Exec(IContext ctx)
        {
            bool cond = ctx.Get<bool>(_condKey);
            object val = cond ? ctx.Get<object>(_thenKey) : ctx.Get<object>(_elseKey);
            return ctx.Set(_out, val);
        }
    }
}
