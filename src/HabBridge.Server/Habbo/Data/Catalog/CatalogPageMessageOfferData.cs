using System.Collections.Generic;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Catalog
{
    public class CatalogPageMessageOfferData
    {
        public CatalogPageMessageOfferData(IPacketReader packet)
        {
            OfferId = packet.NextInt();
            Name = packet.NextString();
            IsRentable = packet.NextBool();
            CostCredits = packet.NextInt();
            CostAdditional = packet.NextInt();
            CostAdditionalType = packet.NextInt();
            IsGiftable = packet.NextBool();

            Products = new List<CatalogPageMessageProductData>(packet.NextInt());
            for (var i = 0; i < Products.Capacity; i++)
            {
                Products.Add(new CatalogPageMessageProductData(packet));
            }

            ClubLevel = packet.NextInt();
            CanSelectAmount = packet.NextBool();
            Unknown0 = packet.NextBool();
            Unknown1 = packet.NextString();
        }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(OfferId);
            packet.Append(Name);
            packet.Append(IsRentable);
            packet.Append(CostCredits);
            packet.Append(CostAdditional);
            packet.Append(CostAdditionalType);
            packet.Append(IsGiftable);

            packet.Append(Products.Count);
            foreach (var product in Products)
            {
                product.WriteTo(packet);
            }

            packet.Append(ClubLevel);
            packet.Append(CanSelectAmount);
            packet.Append(false); // ShowPetPreviewDetails
            packet.Append(Unknown1);
        }

        public int OfferId { get; set; }

        public string Name { get; set; }

        public bool IsRentable { get; set; }
        
        public int CostCredits { get; set; }

        public int CostAdditional { get; set; }

        public int CostAdditionalType { get; set; }

        public bool IsGiftable { get; set; }

        public List<CatalogPageMessageProductData> Products { get; set; }

        public int ClubLevel { get; set; }

        public bool CanSelectAmount { get; set; }

        public bool Unknown0 { get; set; }

        public string Unknown1 { get; set; }
    }
}
