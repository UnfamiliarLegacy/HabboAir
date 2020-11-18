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
    }, Outgoing.FloorHeightMap)]
    public class FloorHeightMapModifier : PacketModifierBase
    {
        public FloorHeightMapModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public bool Unknown0 { get; set; }

        public int WallHeight { get; set; }

        public string Map { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextBool();
            WallHeight = PacketOriginal.NextInt();
            Map = PacketOriginal.NextString();
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);
            PacketModified.Append(WallHeight);
            PacketModified.Append(Map);
        }
    }
}
