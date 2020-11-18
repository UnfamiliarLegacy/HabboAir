using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Inventory.Purse
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.CreditsBalance)]
    public class CreditsBalanceModifier : PacketModifierBase
    {
        public CreditsBalanceModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        // The habbo client parses this to an integer.
        public string Credits { get; set; }

        public override void Parse()
        {
            Credits = PacketOriginal.NextString();
        }

        public override void Recreate()
        {
            PacketModified.Append(Credits);
        }
    }
}
