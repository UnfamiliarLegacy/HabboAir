using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Friendlist
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.SendMsg)]
    public class SendMsgModifier : PacketModifierBase
    {
        public SendMsgModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int UserId { get; set; }

        public string Message { get; set; }

        public override void Parse()
        {
            UserId = PacketOriginal.NextInt();
            Message = PacketOriginal.NextString();
        }

        public override void Recreate()
        {
            PacketModified.Append(UserId);
            PacketModified.Append(Message);
        }
    }
}
