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
    }, Incoming.AcceptFriend)]
    public class AcceptFriendModifier : PacketModifierBase
    {
        public AcceptFriendModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<int> AcceptIds { get; set; }

        public override void Parse()
        {
            AcceptIds = new List<int>(PacketOriginal.NextInt());

            for (var i = 0; i < AcceptIds.Capacity; i++)
            {
                AcceptIds.Add(PacketOriginal.NextInt());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(AcceptIds.Count);

            foreach (var acceptId in AcceptIds)
            {
                PacketModified.Append(acceptId);
            }
        }
    }
}
