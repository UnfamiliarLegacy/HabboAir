using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Notifications
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.ActivityPoints)]
    public class ActivityPointsModifier : PacketModifierBase
    {
        public ActivityPointsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public Dictionary<int, int> Currencies { get; set; }

        public override void Parse()
        {
            Currencies = new Dictionary<int, int>();

            var amount = PacketOriginal.NextInt();

            for (var i = 0; i < amount; i++)
            {
                var currencyId = PacketOriginal.NextInt();
                var currencyValue = PacketOriginal.NextInt();

                Currencies.Add(currencyId, currencyValue);
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Currencies.Count);

            foreach (var (currencyId, currencyValue) in Currencies)
            {
                PacketModified.Append(currencyId);
                PacketModified.Append(currencyValue);
            }
        }
    }
}
