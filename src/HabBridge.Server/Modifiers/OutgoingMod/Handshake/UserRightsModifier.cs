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
    }, Outgoing.UserRights)]
    public class UserRightsModifier : PacketModifierBase
    {
        public UserRightsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int ClubLevel { get; set; }

        public int Rank { get; set; }

        public bool IsAmbassador { get; set; }

        public override void Parse()
        {
            ClubLevel = PacketOriginal.NextInt();
            Rank = PacketOriginal.NextInt();
            IsAmbassador = PacketOriginal.NextBool();
        }

        public override void Recreate()
        {
            PacketModified.Append(ClubLevel);
            PacketModified.Append(Rank);
            PacketModified.Append(IsAmbassador);
        }
    }
}
