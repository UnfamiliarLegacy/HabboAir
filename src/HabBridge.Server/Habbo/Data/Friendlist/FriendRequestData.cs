using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Friendlist
{
    public class FriendRequestData
    {
        public FriendRequestData(IPacketReader packet)
        {
            UserId = packet.NextInt();
            Username = packet.NextString();
            Figure = packet.NextString();
        }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Figure { get; set; }
    }
}
