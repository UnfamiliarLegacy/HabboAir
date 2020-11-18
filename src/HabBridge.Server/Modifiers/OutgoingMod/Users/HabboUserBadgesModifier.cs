using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Users
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.HabboUserBadges)]
    public class HabboUserBadgesModifier : PacketModifierBase
    {
        public HabboUserBadgesModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int UserId { get; set; }

        public Dictionary<int, string> EquippedBadges { get; set; }

        public override void Parse()
        {
            UserId = PacketOriginal.NextInt();

            var equippedCount = PacketOriginal.NextInt();

            EquippedBadges = new Dictionary<int, string>(equippedCount);

            for (var i = 0; i < equippedCount; i++)
            {
                EquippedBadges.Add(PacketOriginal.NextInt(), PacketOriginal.NextString());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(UserId);
            PacketModified.Append(EquippedBadges.Count);

            foreach (var (key, value) in EquippedBadges)
            {
                PacketModified.Append(key);
                PacketModified.Append(value);
            }
        }
    }
}
