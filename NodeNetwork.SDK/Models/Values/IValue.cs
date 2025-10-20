using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models.Values
{
    public interface IDataType {
        string Id { get; }
        bool IsAssignableFrom(IDataType other);
    }

    public interface IValue
    {
        IDataType Type { get; }
    }
}
