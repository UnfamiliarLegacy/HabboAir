using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Friendlist
{
    public class FriendCategoryData
    {
        public FriendCategoryData(IPacketReader packet)
        {
            Id = packet.NextInt();
            Name = packet.NextString();
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
