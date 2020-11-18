using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
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
    }, Outgoing.RoomProperty)]
    public class RoomPropertyModifier : PacketModifierBase
    {
        public RoomPropertyModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public override void Parse()
        {
            Key = PacketOriginal.NextString();
            Value = PacketOriginal.NextString();
        }

        public override void Recreate()
        {
            PacketModified.Append(Key);
            PacketModified.Append(Value);
        }
    }
}
