using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Availability
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.AvailabilityStatus)]
    public class AvailabilityStatusModifier : PacketModifierBase
    {
        public AvailabilityStatusModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public bool Unknown0 { get; set; }

        public bool Unknown1 { get; set; }

        public bool? Unknown2 { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextBool();
            Unknown1 = PacketOriginal.NextBool();
            Unknown2 = PacketOriginal.BytesAvailable ? (bool?) PacketOriginal.NextBool() : null;
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);

            if (Unknown2.HasValue)
            {
                PacketModified.Append(Unknown2.Value);
            }
        }
    }
}
