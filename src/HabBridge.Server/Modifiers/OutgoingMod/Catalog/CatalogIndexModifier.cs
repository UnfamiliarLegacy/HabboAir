using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Catalog;
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
    }, Outgoing.CatalogIndex)]
    public class CatalogIndexModifier : PacketModifierBase
    {
        public CatalogIndexModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }
        
        public NodeData Root { get; set; }

        public bool Unknown0 { get; set; }

        public string CatalogType { get; set; }

        public override void Parse()
        {
            Root = new NodeData(PacketOriginal);
            Unknown0 = PacketOriginal.NextBool();
            CatalogType = PacketOriginal.NextString();
        }

        public override void Recreate()
        {
            Root.WriteTo(PacketModified);
            PacketModified.Append(Unknown0);
            PacketModified.Append(CatalogType);
        }
    }
}
