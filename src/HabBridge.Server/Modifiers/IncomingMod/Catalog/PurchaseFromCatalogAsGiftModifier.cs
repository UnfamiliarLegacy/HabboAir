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
    }, Incoming.PurchaseFromCatalogAsGift)]
    public class PurchaseFromCatalogAsGiftModifier : PacketModifierBase
    {
        public PurchaseFromCatalogAsGiftModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int PageId { get; set; }

        public int ItemId { get; set; }

        public string ExtraData { get; set; }

        public string GiftUser { get; set; }

        public string GiftMessage { get; set; }

        public int SpriteId { get; set; }

        public int Ribbon { get; set; }

        public int Colour { get; set; }

        public bool Unknown0 { get; set; }

        public override void Parse()
        {
            PageId = PacketOriginal.NextInt();
            ItemId = PacketOriginal.NextInt();
            ExtraData = PacketOriginal.NextString();
            GiftUser = PacketOriginal.NextString();
            GiftMessage = PacketOriginal.NextString();
            SpriteId = PacketOriginal.NextInt();
            Ribbon = PacketOriginal.NextInt();
            Colour = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.NextBool();
        }

        public override void Recreate()
        {
            PacketModified.Append(PageId);
            PacketModified.Append(ItemId);
            PacketModified.Append(ExtraData);
            PacketModified.Append(GiftUser);
            PacketModified.Append(GiftMessage);
            PacketModified.Append(SpriteId);
            PacketModified.Append(Ribbon);
            PacketModified.Append(Colour);
            PacketModified.Append(Unknown0);
        }
    }
}
