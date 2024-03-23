using System;
using System.Runtime.Serialization;

namespace Thorium.Shared.FunctionServer.Tcp
{
    [Serializable]
    internal class FunctionNotFoundException : Exception
    {
        public FunctionNotFoundException()
        {
        }

        public FunctionNotFoundException(string message) : base(message)
        {
        }

        public FunctionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FunctionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}