using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Navigator
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.UpdateHomeRoom)]
    public class UpdateHomeRoomModifier : PacketModifierBase
    {
        public UpdateHomeRoomModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        /// <summary>
        ///     Use -1 to disable home room.
        /// </summary>
        public int RoomId { get; set; }

        public override void Parse()
        {
            RoomId = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomId);
        }
    }
}
