using System;
using System.Runtime.Serialization;

namespace HabBridge.Server.Registers.Exceptions
{
    public class UnknownPacketHeaderException : Exception
    {
        public UnknownPacketHeaderException()
        {
        }

        public UnknownPacketHeaderException(string message) : base(message)
        {
        }

        public UnknownPacketHeaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownPacketHeaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
