using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Friendlist
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.DeclineFriend)]
    public class DeclineFriendModifier : PacketModifierBase
    {
        public DeclineFriendModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public bool DeclineAll { get; set; }

        public List<int> DeclineIds { get; set; }

        public override void Parse()
        {
            DeclineAll = PacketOriginal.NextBool();
            DeclineIds = new List<int>(PacketOriginal.NextInt());

            for (var i = 0; i < DeclineIds.Capacity; i++)
            {
                DeclineIds.Add(PacketOriginal.NextInt());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(DeclineAll);
            PacketModified.Append(DeclineIds.Count);

            foreach (var declineId in DeclineIds)
            {
                PacketModified.Append(declineId);
            }
        }
    }
}
