using System;
using System.Collections.Generic;
using System.Globalization;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Room.Engine;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Room.Engine
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.Users)]
    public class UsersModifier : PacketModifierBase
    {
        public UsersModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<RoomUser> RoomUsers { get; set; }

        public override void Parse()
        {
            RoomUsers = new List<RoomUser>(PacketOriginal.NextInt());

            for (var i = 0; i < RoomUsers.Capacity; i++)
            {
                RoomUsers.Add(new RoomUser(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomUsers.Count);

            foreach (var roomUser in RoomUsers)
            {
                PacketModified.Append(roomUser.UserId);
                PacketModified.Append(roomUser.DisplayName);
                PacketModified.Append(roomUser.Motto);
                PacketModified.Append(roomUser.Figure);
                PacketModified.Append(roomUser.VirtualId);
                PacketModified.Append(roomUser.X);
                PacketModified.Append(roomUser.Y);
                PacketModified.Append(roomUser.Z.ToString(CultureInfo.InvariantCulture));
                PacketModified.Append(roomUser.Unknown0);
                PacketModified.Append((int) roomUser.Unknown1);

                switch (roomUser.Unknown1)
                {
                    case RoomUserType.User:
                        PacketModified.Append(roomUser.UserGender);
                        PacketModified.Append(roomUser.UserUnknown0);
                        PacketModified.Append(roomUser.UserUnknown1);
                        PacketModified.Append(roomUser.UserUnknown2);
                        PacketModified.Append(roomUser.UserUnknown3);
                        PacketModified.Append(roomUser.UserUnknown4);
                        PacketModified.Append(roomUser.UserUnknown5);
                        break;
                    case RoomUserType.Pet:
                        PacketModified.Append(roomUser.PetUnknown0);
                        PacketModified.Append(roomUser.PetUnknown1);
                        PacketModified.Append(roomUser.PetUnknown2);
                        PacketModified.Append(roomUser.PetUnknown3);
                        PacketModified.Append(roomUser.PetUnknown4);
                        PacketModified.Append(roomUser.PetUnknown5);
                        PacketModified.Append(roomUser.PetUnknown6);
                        PacketModified.Append(roomUser.PetUnknown7);
                        PacketModified.Append(roomUser.PetUnknown8);
                        PacketModified.Append(roomUser.PetUnknown9);
                        PacketModified.Append(roomUser.PetUnknown10);
                        PacketModified.Append(roomUser.PetUnknown11);
                        break;
                    case RoomUserType.Unknown1:
                        // Nothing.
                        break;
                    case RoomUserType.Bot:
                        PacketModified.Append(roomUser.BotUnknown0);
                        PacketModified.Append(roomUser.BotUnknown1);
                        PacketModified.Append(roomUser.BotUnknown2);
                        PacketModified.Append(roomUser.BotUnknown3.Count);

                        foreach (var unknown in roomUser.BotUnknown3)
                        {
                            PacketModified.Append(unknown);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
