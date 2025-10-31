using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.CustomException
{
    public class InvalidKeyException : Exception
    {
        public ErrorCode ErrorCode { get; }
        public string FieldName { get; }

        public InvalidKeyException(string fieldName)
        {
            FieldName = fieldName;
            ErrorCode = ErrorCode.GraphInvalid;
        }
        public InvalidKeyException(string message, string FieldName, ErrorCode errorCode)
            : base(message)
        {
            this.FieldName = FieldName;
            this.ErrorCode = errorCode;
        }
        public InvalidKeyException(string? message, string FieldName, ErrorCode errorCode, Exception? innerException)
            : base(message, innerException)
        {
            this.FieldName = FieldName;
            this.ErrorCode = errorCode;
        }
    }
}
