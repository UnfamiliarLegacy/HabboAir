using System;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Net.Packet.Events
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public IPacket Packet { get; set; }
    }
}
