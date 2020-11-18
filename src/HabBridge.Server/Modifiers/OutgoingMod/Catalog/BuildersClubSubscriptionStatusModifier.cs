using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Catalog
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.BuildersClubSubscriptionStatus)]
    public class BuildersClubSubscriptionStatusModifier : PacketModifierBase
    {
        public BuildersClubSubscriptionStatusModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Unknown0 { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int? Unknown3 { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextInt();
            Unknown2 = PacketOriginal.NextInt();
            Unknown3 = PacketOriginal.BytesAvailable ? (int?) PacketOriginal.NextInt() : null;
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Unknown2);

            if (Unknown3.HasValue)
            {
                PacketModified.Append(Unknown3.Value);
            }
        }
    }
}
