using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Competition
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.GetCurrentTimingCode)]
    public class GetCurrentTimingCodeModifier : PacketModifierBase
    {
        public GetCurrentTimingCodeModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string Config { get; set; }

        public override void Parse()
        {
            // If we target Leet.
            if (PacketOriginal.Release == Release.AIR63_201911271159_623255659 &&
                PacketModified.Release == Release.PRODUCTION_201701242205_837386174 &&
                PacketOriginal.BytesLeft == 0)
            {
                Config = string.Empty;
            }
            else
            {
                Config = PacketOriginal.NextString();
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Config);
        }
    }
}
