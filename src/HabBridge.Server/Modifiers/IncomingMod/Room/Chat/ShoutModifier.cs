using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Room.Chat
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.Shout)]
    public class ShoutModifier : PacketModifierBase
    {
        public ShoutModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string Message { get; set; }

        public int Colour { get; set; }

        public override void Parse()
        {
            Message = PacketOriginal.NextString();
            Colour = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(Message);
            PacketModified.Append(Colour);
        }
    }
}
