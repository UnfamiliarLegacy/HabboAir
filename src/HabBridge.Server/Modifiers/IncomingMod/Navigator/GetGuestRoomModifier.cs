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
    }, Incoming.GetGuestRoom)]
    public class GetGuestRoomModifier : PacketModifierBase
    {
        public GetGuestRoomModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int RoomId { get; set; }

        public int IsLoading { get; set; }

        public int CheckEntry { get; set; }

        public override void Parse()
        {
            RoomId = PacketOriginal.NextInt();
            IsLoading = PacketOriginal.NextInt();
            CheckEntry = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomId);
            PacketModified.Append(IsLoading);
            PacketModified.Append(CheckEntry);
        }
    }
}
