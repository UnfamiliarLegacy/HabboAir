using System.Collections.Generic;
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
    }, Outgoing.HeightMap)]
    public class HeightMapModifier : PacketModifierBase
    {
        public HeightMapModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Width { get; set; }

        public List<short> Entries { get; set; }

        public override void Parse()
        {
            Width = PacketOriginal.NextInt();
            Entries = new List<short>(PacketOriginal.NextInt());

            for (var i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(PacketOriginal.NextShort());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Width);
            PacketModified.Append(Entries.Count);

            foreach (var entry in Entries)
            {
                PacketModified.Append(entry);
            }
        }
    }
}
