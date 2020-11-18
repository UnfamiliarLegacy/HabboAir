using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Room.Session
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.RoomReady)]
    public class RoomReadyModifier : PacketModifierBase
    {
        public RoomReadyModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string Model { get; set; }

        public int RoomId { get; set; }

        public override void Parse()
        {
            Model = PacketOriginal.NextString();
            RoomId = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(Model);
            PacketModified.Append(RoomId);
        }
    }
}
