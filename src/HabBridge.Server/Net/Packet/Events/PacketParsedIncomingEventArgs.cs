using HabBridge.Server.Habbo;

namespace HabBridge.Server.Net.Packet.Events
{
    public class PacketParsedIncomingEventArgs : PacketParsedEventArgs
    {
        public Incoming Header { get; set; }
    }
}
