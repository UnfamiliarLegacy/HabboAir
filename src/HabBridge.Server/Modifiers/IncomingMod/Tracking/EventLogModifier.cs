using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Tracking
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.EventLog)]
    public class EventLogModifier : PacketModifierBase
    {
        public EventLogModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string Unknown0 { get; set; }

        public string Unknown1 { get; set; }

        public string Unknown2 { get; set; }

        public string Unknown3 { get; set; }

        public int Unknown4 { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextString();
            Unknown1 = PacketOriginal.NextString();
            Unknown2 = PacketOriginal.NextString();
            Unknown3 = PacketOriginal.NextString();
            Unknown4 = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Unknown2);
            PacketModified.Append(Unknown3);
            PacketModified.Append(Unknown4);
        }
    }
}
