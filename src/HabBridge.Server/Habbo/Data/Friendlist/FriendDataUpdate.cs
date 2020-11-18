using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Friendlist
{
    public class FriendDataUpdate
    {
        public FriendDataUpdate(IPacketReader packet)
        {
            Type = packet.NextInt();

            if (Type == -1)
            {
                FriendId = packet.NextInt();
            } else {
                Data = new FriendData(packet);
            }
        }

        public int Type { get; set; }

        public int FriendId { get; set; }

        public FriendData Data { get; set; }
    }
}
