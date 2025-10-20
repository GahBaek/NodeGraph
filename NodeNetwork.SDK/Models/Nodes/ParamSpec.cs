using NodeNetworkSDK.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Nodes
{
    public sealed class ParamSpec
    {
        public string Name { get; }
        public IDataType Type { get; }
        public bool Required { get; }
        public string? Description { get; }
        public ParamSpec(string name, IDataType type, bool required = true, string? desc = null)
        { Name = name; Type = type; Required = required; Description = desc; }
    }
}
