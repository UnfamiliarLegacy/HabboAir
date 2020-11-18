using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Catalog
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.BundleDiscountRuleset)]
    public class BundleDiscountRulesetModifier : PacketModifierBase
    {
        public BundleDiscountRulesetModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Unknown0 { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int Unknown3 { get; set; }

        public List<int> Unknown4 { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextInt();
            Unknown2 = PacketOriginal.NextInt();
            Unknown3 = PacketOriginal.NextInt();
            Unknown4 = new List<int>(PacketOriginal.NextInt());

            for (var i = 0; i < Unknown4.Capacity; i++)
            {
                Unknown4.Add(PacketOriginal.NextInt());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Unknown2);
            PacketModified.Append(Unknown3);
            PacketModified.Append(Unknown4.Count);

            foreach (var unknown in Unknown4)
            {
                PacketModified.Append(unknown);
            }
        }
    }
}
