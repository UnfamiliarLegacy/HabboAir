using System;
using System.Runtime.Serialization;

namespace HabBridge.Server.Net.Packet.Exceptions
{
    public class PacketUnreadableException : Exception
    {
        public PacketUnreadableException()
        {
        }

        public PacketUnreadableException(string message) : base(message)
        {
        }

        public PacketUnreadableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PacketUnreadableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
