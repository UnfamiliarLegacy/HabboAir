using System;
using System.Runtime.Serialization;

namespace HabBridge.Server.Net.Packet.Exceptions
{
    public class UnidentifiedPacketModifierException : Exception
    {
        public UnidentifiedPacketModifierException()
        {
        }

        public UnidentifiedPacketModifierException(string message) : base(message)
        {
        }

        public UnidentifiedPacketModifierException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnidentifiedPacketModifierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
