using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Handshake
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.UserObject)]
    public class UserObjectModifier : PacketModifierBase
    {
        public UserObjectModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Look { get; set; }

        public string Gender { get; set; }

        public string Motto { get; set; }

        public string Unknown0 { get; set; } // Friendship related

        public bool Unknown1 { get; set; }

        public int Respect { get; set; }

        public int DailyRespectPoints { get; set; }

        public int DailyPetRespectPoints { get; set; }

        public bool FriendStreamActive { get; set; }

        public string LastOnline { get; set; }

        public bool CanChangeName { get; set; }

        public bool Unknown2 { get; set; }

        public override void Parse()
        {
            Id = PacketOriginal.NextInt();
            Username = PacketOriginal.NextString();
            Look = PacketOriginal.NextString();
            Gender = PacketOriginal.NextString();
            Motto = PacketOriginal.NextString();
            Unknown0 = PacketOriginal.NextString();
            Unknown1 = PacketOriginal.NextBool();
            Respect = PacketOriginal.NextInt();
            DailyRespectPoints = PacketOriginal.NextInt();
            DailyPetRespectPoints = PacketOriginal.NextInt();
            FriendStreamActive = PacketOriginal.NextBool();
            LastOnline = PacketOriginal.NextString();
            CanChangeName = PacketOriginal.NextBool();
            Unknown2 = PacketOriginal.NextBool();
        }

        public override void Recreate()
        {
            PacketModified.Append(Id);
            PacketModified.Append(Username);
            PacketModified.Append(Look);
            PacketModified.Append(Gender);
            PacketModified.Append(Motto);
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Respect);
            PacketModified.Append(DailyRespectPoints);
            PacketModified.Append(DailyPetRespectPoints);
            PacketModified.Append(FriendStreamActive);
            PacketModified.Append(LastOnline);
            PacketModified.Append(CanChangeName);
            PacketModified.Append(Unknown2);
        }
    }
}
