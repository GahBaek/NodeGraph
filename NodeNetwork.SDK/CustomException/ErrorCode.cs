using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.CustomException
{
    public readonly record struct ErrorCode(string Value)
    {
        public override string ToString() => Value;

        public static readonly ErrorCode GeneralException = new("General_Exception");
        public static readonly ErrorCode GraphInvalid = new("Invalid_Graph");
        public static readonly ErrorCode InvalidInput = new("Invalid_Input");
        public static readonly ErrorCode InvalidOutput = new("Invalid_Output");
        public static readonly ErrorCode InvalidGraphId = new("Invalid_GraphId");
        public static readonly ErrorCode InvalidValue = new("Invalid_Value");

        public static implicit operator string(ErrorCode c) => c.Value;
    }
}
