﻿using HabBridge.Habbo.Shared;
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
    }, Outgoing.HabboActivityPointNotification)]
    public class HabboActivityPointNotificationModifier : PacketModifierBase
    {
        public HabboActivityPointNotificationModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Amount { get; set; }

        public int Change { get; set; }

        public int Type { get; set; }

        public override void Parse()
        {
            Amount = PacketOriginal.NextInt();
            Change = PacketOriginal.NextInt();
            Type = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(Amount);
            PacketModified.Append(Change);
            PacketModified.Append(Type);
        }
    }
}
