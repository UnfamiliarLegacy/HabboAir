using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Friendlist
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.NewConsoleMessage)]
    public class NewConsoleMessageModifier : PacketModifierBase
    {
        public NewConsoleMessageModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int SenderUserId { get; set; }

        public string Message { get; set; }

        public int Timestamp { get; set; }

        public string Unknown0 { get; set; }

        public override void Parse()
        {
            SenderUserId = PacketOriginal.NextInt();
            Message = PacketOriginal.NextString();
            Timestamp = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.BytesAvailable ? PacketOriginal.NextString() : null;
        }

        public override void Recreate()
        {
            PacketModified.Append(SenderUserId);
            PacketModified.Append(Message);
            PacketModified.Append(Timestamp);

            if (Unknown0 != null)
            {
                PacketModified.Append(Unknown0);
            }
        }
    }
}
