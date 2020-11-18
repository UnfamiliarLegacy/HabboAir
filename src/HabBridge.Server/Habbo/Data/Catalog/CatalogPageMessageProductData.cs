using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Catalog
{
    public class CatalogPageMessageProductData
    {
        public CatalogPageMessageProductData(IPacketReader packet)
        {
            ItemType = packet.NextString();

            switch (ItemType)
            {
                case "b":
                    Name = packet.NextString();
                    Amount = 1;
                    break;
                default:
                    SpriteId = packet.NextInt();
                    Name = packet.NextString();
                    Amount = packet.NextInt();
                    IsLimited = packet.NextBool();

                    if (IsLimited)
                    {
                        LimitedEditionStack = packet.NextInt();
                        LimitedEditionSells = packet.NextInt();
                    }
                    break;
            }
        }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(ItemType);

            if (!ItemType.Equals("b"))
            {
                packet.Append(SpriteId);
                packet.Append(Name);
                packet.Append(Amount);
                packet.Append(IsLimited);

                if (IsLimited)
                {
                    packet.Append(LimitedEditionStack);
                    packet.Append(LimitedEditionSells);
                }
            }
            else
            {
                packet.Append(Name);
            }
        }

        public string ItemType { get; set; }

        public int SpriteId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public bool IsLimited { get; set; }

        public int LimitedEditionStack { get; set; }

        public int LimitedEditionSells { get; set; }
    }
}
