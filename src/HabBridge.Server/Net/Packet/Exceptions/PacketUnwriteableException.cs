using System;
using System.Runtime.Serialization;

namespace HabBridge.Server.Net.Packet.Exceptions
{
    public class PacketUnwriteableException : Exception
    {
        public PacketUnwriteableException()
        {
        }

        public PacketUnwriteableException(string message) : base(message)
        {
        }

        public PacketUnwriteableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PacketUnwriteableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
