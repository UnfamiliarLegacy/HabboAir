using System;
using HabBridge.Server.Modifiers;

namespace HabBridge.Server.Net.Packet.Events
{
    public class PacketParsedEventArgs : EventArgs
    {
        public PacketModifierBase PacketModifier { get; set; }
    }
}
