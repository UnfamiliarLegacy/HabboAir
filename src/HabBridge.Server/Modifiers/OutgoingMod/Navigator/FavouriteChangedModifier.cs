﻿using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Navigator
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.FavouriteChanged)]
    public class FavouriteChangedModifier : PacketModifierBase
    {
        public FavouriteChangedModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int RoomId { get; set; }

        public bool IsAdded { get; set; }

        public override void Parse()
        {
            RoomId = PacketOriginal.NextInt();
            IsAdded = PacketOriginal.NextBool();
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomId);
            PacketModified.Append(IsAdded);
        }
    }
}
