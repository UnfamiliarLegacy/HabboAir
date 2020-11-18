using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.RoomSettings
{
    public class RoomChatSettings
    {
        public RoomChatSettings(IPacketReader packet)
        {
            ChatMode = packet.NextInt();
            ChatSize = packet.NextInt();
            ChatSpeed = packet.NextInt();
            ChatDistance = packet.NextInt();
            ExtraFlood = packet.NextInt();
        }

        public int ChatMode { get; set; }

        public int ChatSize { get; set; }

        public int ChatSpeed { get; set; }

        public int ChatDistance { get; set; }

        public int ExtraFlood { get; set; }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(ChatMode);
            packet.Append(ChatSize);
            packet.Append(ChatSpeed);
            packet.Append(ChatDistance);
            packet.Append(ExtraFlood);
        }
    }
}
