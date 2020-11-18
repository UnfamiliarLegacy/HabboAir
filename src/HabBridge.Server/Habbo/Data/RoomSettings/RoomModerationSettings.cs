using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.RoomSettings
{
    public class RoomModerationSettings
    {
        public RoomModerationSettings(IPacketReader packet)
        {
            WhoMute = packet.NextInt();
            WhoKick = packet.NextInt();
            WhoBan = packet.NextInt();
        }

        public int WhoMute { get; set; }

        public int WhoKick { get; set; }

        public int WhoBan { get; set; }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(WhoMute);
            packet.Append(WhoKick);
            packet.Append(WhoBan);
        }
    }
}
