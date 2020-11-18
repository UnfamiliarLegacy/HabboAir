using System.Collections.Generic;
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
    }, Outgoing.CatalogPage)]
    public class CatalogPageModifier : PacketModifierBase
    {
        public CatalogPageModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int PageId { get; set; }

        public string CatalogType { get; set; }

        public string LayoutCode { get; set; }

        public CatalogLocalizationData Localization { get; set; }

        public List<CatalogPageMessageOfferData> Offers { get; set; }

        public int OfferId { get; set; }

        public bool Unknown1 { get; set; }

        public List<FrontPageItem> FrontPageItems { get; set; }

        public override void Parse()
        {
            PageId = PacketOriginal.NextInt();
            CatalogType = PacketOriginal.NextString();
            LayoutCode = PacketOriginal.NextString();
            Localization = new CatalogLocalizationData(PacketOriginal);

            Offers = new List<CatalogPageMessageOfferData>(PacketOriginal.NextInt());
            for (var i = 0; i < Offers.Capacity; i++)
            {
                Offers.Add(new CatalogPageMessageOfferData(PacketOriginal));
            }

            OfferId = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextBool();

            if (PacketOriginal.BytesAvailable)
            {
                FrontPageItems = new List<FrontPageItem>(PacketOriginal.NextInt());
                for (var i = 0; i < FrontPageItems.Capacity; i++)
                {
                    FrontPageItems.Add(new FrontPageItem(PacketOriginal));
                }
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(PageId);
            PacketModified.Append(CatalogType);
            PacketModified.Append(LayoutCode);

            Localization.WriteTo(PacketModified);

            PacketModified.Append(Offers.Count);
            foreach (var offer in Offers)
            {
                offer.WriteTo(PacketModified);
            }

            PacketModified.Append(OfferId);
            PacketModified.Append(Unknown1);

            if (FrontPageItems != null)
            {
                PacketModified.Append(FrontPageItems.Count);
                foreach (var item in FrontPageItems)
                {
                    item.WriteTo(PacketModified);
                }
            }
        }
    }
}
