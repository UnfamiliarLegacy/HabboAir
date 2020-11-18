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
    }, Outgoing.UserUpdate)]
    public class UserUpdateModifier : PacketModifierBase
    {
        public UserUpdateModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<RoomUserUpdate> RoomUserUpdates { get; set; }

        public override void Parse()
        {
            RoomUserUpdates = new List<RoomUserUpdate>(PacketOriginal.NextInt());

            for (var i = 0; i < RoomUserUpdates.Capacity; i++)
            {
                RoomUserUpdates.Add(new RoomUserUpdate(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomUserUpdates.Count);

            foreach (var update in RoomUserUpdates)
            {
                PacketModified.Append(update.VirtualId);
                PacketModified.Append(update.X);
                PacketModified.Append(update.Y);
                PacketModified.Append(update.Z.ToString(CultureInfo.InvariantCulture));
                PacketModified.Append(update.RotHead);
                PacketModified.Append(update.RotBody);
                PacketModified.Append(update.Status);
            }
        }
    }
}
