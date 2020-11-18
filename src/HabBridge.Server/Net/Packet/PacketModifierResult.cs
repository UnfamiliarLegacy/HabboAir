using System.Collections.Generic;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Net.Packet
{
    public class PacketModifierResult<T> where T : IPacket
    {
        public PacketModifierResult(T packetModified, List<T> extraPackets = null)
        {
            PacketModified = packetModified;
            ExtraPackets = extraPackets;
        }
        
        public T PacketModified { get; }
        public List<T> ExtraPackets { get; }
    }
}