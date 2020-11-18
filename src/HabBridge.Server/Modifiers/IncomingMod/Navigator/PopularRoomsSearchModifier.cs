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
    }, Incoming.PopularRoomsSearch)]
    public class PopularRoomsSearchModifier : PacketModifierBase
    {
        private const string Category = "hotel_view";

        public PopularRoomsSearchModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string CategoryId { get; set; }

        public override void Parse()
        {
            CategoryId = PacketOriginal.NextString();
            PacketOriginal.NextInt();

            if (CategoryId.Equals("-1"))
            {
                Connection.Session.Navigator.SetLastSearch(Category, "popular", NavigatorSearchType.HotelView);
            }
            else if (int.TryParse(CategoryId, out var categoryId) && Connection.Session.Navigator.UserFlatCatMapping.ContainsKey(categoryId))
            {
                Connection.Session.Navigator.SetLastSearch(Category, Connection.Session.Navigator.UserFlatCatMapping[categoryId], NavigatorSearchType.HotelViewUserCat);
            }
            else
            {
                Discard = true;
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Connection.Session.Navigator.LastCategorySearch);
            PacketModified.Append(string.Empty);
        }
    }
}
