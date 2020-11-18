using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Session.Navigator;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Navigator
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.RoomAdSearch)]
    public class RoomAdSearchModifier : PacketModifierBase
    {
        private const string Category = "roomads_view";

        private const string CategoryId = "top_promotions";

        public RoomAdSearchModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int TypeId { get; set; }

        public override void Parse()
        {
            PacketOriginal.NextInt();
            TypeId = PacketOriginal.NextInt();

            Connection.Session.Navigator.SetLastSearch(Category, CategoryId, TypeId == 16 
                ? NavigatorSearchType.PromotionHot 
                : NavigatorSearchType.PromotionNew);
        }

        public override void Recreate()
        {
            PacketModified.Append(Connection.Session.Navigator.LastCategorySearch);
            PacketModified.Append(string.Empty);
        }
    }
}
