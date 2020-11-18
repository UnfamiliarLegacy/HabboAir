using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Navigator
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.RoomEvent)]
    public class RoomEventModifier : PacketModifierBase
    {
        public RoomEventModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        public int Unknown0 { get; set; }

        public int Unknown1 { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Unknown2 { get; set; }

        public int Unknown3 { get; set; }

        public int Unknown4 { get; set; }

        public override void Parse()
        {
            Id = PacketOriginal.NextInt();
            OwnerId = PacketOriginal.NextInt();
            OwnerName = PacketOriginal.NextString();
            Unknown0 = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextInt();
            Name = PacketOriginal.NextString();
            Description = PacketOriginal.NextString();
            Unknown2 = PacketOriginal.NextInt();
            Unknown3 = PacketOriginal.NextInt();
            Unknown4 = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(Id);
            PacketModified.Append(OwnerId);
            PacketModified.Append(OwnerName);
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Name);
            PacketModified.Append(Description);
            PacketModified.Append(Unknown2);
            PacketModified.Append(Unknown3);
            PacketModified.Append(Unknown4);
        }
    }
}
