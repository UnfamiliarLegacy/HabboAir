using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Friendlist
{
    public class FriendData
    {
        public FriendData(IPacketReader PacketOriginal)
        {
            Id = PacketOriginal.NextInt();
            Username = PacketOriginal.NextString();
            Gender = PacketOriginal.NextInt();
            Online = PacketOriginal.NextBool();
            InRoom = PacketOriginal.NextBool();
            Figure = PacketOriginal.NextString();
            CategoryId = PacketOriginal.NextInt();
            Motto = PacketOriginal.NextString();
            FacebookUsername = PacketOriginal.NextString();
            Unknown1 = PacketOriginal.NextString();
            AllowOfflineMessaging = PacketOriginal.NextBool();
            Unknown3 = PacketOriginal.NextBool();
            IsMobileUser = PacketOriginal.NextBool();
            RelationshipStatus = PacketOriginal.NextShort();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public int Gender { get; set; }

        public bool Online { get; set; }

        public bool InRoom { get; set; }

        public string Figure { get; set; }

        public int CategoryId { get; set; }

        public string Motto { get; set; }

        public string FacebookUsername { get; set; }

        public string Unknown1 { get; set; }

        public bool AllowOfflineMessaging { get; set; }

        public bool Unknown3 { get; set; }

        public bool IsMobileUser { get; set; }

        public short RelationshipStatus { get; set; }
    }
}
