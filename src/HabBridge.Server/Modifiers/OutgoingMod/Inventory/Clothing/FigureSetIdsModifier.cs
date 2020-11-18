using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Inventory.Clothing
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.FigureSetIds)]
    public class FigureSetIdsModifier : PacketModifierBase
    {
        public FigureSetIdsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<int> PartIds { get; set; }

        public List<string> Parts { get; set; }

        public override void Parse()
        {
            PartIds = new List<int>(PacketOriginal.NextInt());

            for (var i = 0; i < PartIds.Capacity; i++)
            {
                PartIds.Add(PacketOriginal.NextInt());
            }

            Parts = new List<string>(PacketOriginal.NextInt());

            for (var i = 0; i < Parts.Capacity; i++)
            {
                Parts.Add(PacketOriginal.NextString());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(PartIds.Count);

            foreach (var partId in PartIds)
            {
                PacketModified.Append(partId);
            }

            PacketModified.Append(Parts.Count);

            foreach (var part in Parts)
            {
                PacketModified.Append(part);
            }
        }
    }
}
