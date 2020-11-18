using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Catalog
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.GetCatalogPage)]
    public class GetCatalogPageModifier : PacketModifierBase
    {
        public GetCatalogPageModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int PageId { get; set; }

        // Always set to -1.
        public int Unknown0 { get; set; }

        public string CatalogMode { get; set; }

        public override void Parse()
        {
            PageId = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.NextInt();
            CatalogMode = PacketOriginal.NextString();
        }

        public override void Recreate()
        {
            PacketModified.Append(PageId);
            PacketModified.Append(Unknown0);
            PacketModified.Append(CatalogMode);
        }
    }
}
