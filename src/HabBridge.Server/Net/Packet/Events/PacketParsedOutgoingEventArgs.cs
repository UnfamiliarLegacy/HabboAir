using HabBridge.Server.Habbo;

namespace HabBridge.Server.Net.Packet.Events
{
    public class PacketParsedOutgoingEventArgs : PacketParsedEventArgs
    {
        public Outgoing Header { get; set; }
    }
}
