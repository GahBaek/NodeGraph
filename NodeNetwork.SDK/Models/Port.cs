using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public class Port
    {
        public Type DataType { get; }
        public bool AllowMultiple { get; } 
        public Port(Type dataType, bool allowMultiple = false)
        { DataType = dataType; AllowMultiple = allowMultiple; }
    }
}
