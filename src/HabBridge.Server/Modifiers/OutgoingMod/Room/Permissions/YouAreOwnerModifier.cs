﻿using System;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Room.Permissions
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.YouAreOwner)]
    public class YouAreOwnerModifier : PacketModifierBase
    {
        public YouAreOwnerModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }
        
        public int? FlatId { get; set; }

        public override void Parse()
        {
            if (ReleaseFrom >= Release.PRODUCTION_201701242205_837386174)
            {
                FlatId = PacketOriginal.NextInt();
            }
        }

        public override void Recreate()
        {
            // HabboAir expects RoomId.
            if (FlatId.HasValue)
            {
                PacketModified.Append(FlatId.Value);
            }
            else
            {
                if (!Connection.Session.Room.LastFlatConnectionRoomId.HasValue)
                {
                    throw new ApplicationException($"{Outgoing.YouAreOwner.ToString()} was called without an incoming {Incoming.OpenFlatConnection.ToString()} packet beforehand.");
                }

                PacketModified.Append(Connection.Session.Room.LastFlatConnectionRoomId.Value);
            }
        }
    }
}
