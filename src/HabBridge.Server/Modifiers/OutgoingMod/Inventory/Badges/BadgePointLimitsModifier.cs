using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Inventory.Badges;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Inventory.Badges
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.BadgePointLimits)]
    public class BadgePointLimitsModifier : PacketModifierBase
    {
        public BadgePointLimitsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<BadgeData> Badges { get; set; }

        public override void Parse()
        {
            Badges = new List<BadgeData>(PacketOriginal.NextInt());

            for (var i = 0; i < Badges.Capacity; i++)
            {
                var group = new BadgeData
                {
                    GroupName = PacketOriginal.NextString(),
                    Levels = new List<BadgePointLimitData>(PacketOriginal.NextInt())
                };

                for (var j = 0; j < group.Levels.Capacity; j++)
                {
                    group.Levels.Add(new BadgePointLimitData
                    {
                        Level = PacketOriginal.NextInt(),
                        Requirement = PacketOriginal.NextInt()
                    });
                }

                Badges.Add(group);
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Badges.Count);

            foreach (var badge in Badges)
            {
                PacketModified.Append(badge.GroupName);
                PacketModified.Append(badge.Levels.Count);

                foreach (var badgeLevel in badge.Levels)
                {
                    PacketModified.Append(badgeLevel.Level);
                    PacketModified.Append(badgeLevel.Requirement);
                }
            }
        }
    }
}
