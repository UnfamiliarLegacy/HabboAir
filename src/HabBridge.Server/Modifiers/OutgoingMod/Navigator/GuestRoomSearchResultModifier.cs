using System.Collections.Generic;
using System.Linq;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Extensions;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Navigator;
using HabBridge.Server.Habbo.Session.Navigator;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;
using HabBridge.Server.Utils;

namespace HabBridge.Server.Modifiers.OutgoingMod.Navigator
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.GuestRoomSearchResult)]
    public class GuestRoomSearchResultModifier : PacketModifierBase
    {
        public GuestRoomSearchResultModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string SearchCode { get; set; }

        public string Text { get; set; }

        public List<WebSearchResultList> SearchResults { get; set; }

        public override void Parse()
        {
            // Habbo web specific.
            SearchCode = PacketOriginal.NextString();
            Text = PacketOriginal.NextString();
            SearchResults = new List<WebSearchResultList>(PacketOriginal.NextInt());

            for (var i = 0; i < SearchResults.Capacity; i++)
            {
                SearchResults.Add(new WebSearchResultList(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            if (!Connection.Session.Navigator.LastCategorySearch.Equals(SearchCode))
            {
                Discard = true;
                return;
            }
            
            PacketModified.Append(0); // Ignored.
            PacketModified.Append("");

            List<WebRoomData> rooms = null;

            switch (Connection.Session.Navigator.LastCategorySearchType)
            {
                case NavigatorSearchType.MyWorldView:
                case NavigatorSearchType.HotelView:
                case NavigatorSearchType.PromotionHot:
                    rooms = SearchResults.FirstOrDefault(x => x.CategoryId.Equals(Connection.Session.Navigator.LastCategorySearchId))?.GuestRooms;
                    break;
                case NavigatorSearchType.MyWorldViewHighestScore:
                    rooms = SearchResults.SelectMany(x => x.GuestRooms).DistinctBy(x => x.FlatId).OrderByDescending(x => x.Score).ToList();
                    break;
                case NavigatorSearchType.HotelViewUserCat:
                    rooms = SearchResults
                        .OrderBy(x => StringUtils.DamerauLevenshteinDistance(x.PublicName, Connection.Session.Navigator.LastCategorySearchId, 50))
                        .FirstOrDefault()?
                        .GuestRooms;
                    break;
                case NavigatorSearchType.PromotionNew:
                    rooms = SearchResults.SelectMany(x => x.GuestRooms).DistinctBy(x => x.FlatId).OrderByDescending(x => x.FlatId).ToList();
                    break;
                case NavigatorSearchType.OfficialView:
                    rooms = SearchResults.FirstOrDefault()?.GuestRooms;
                    break;
            }

            if (rooms == null)
            {
                Discard = true;
                return;
            }

            PacketModified.Append(rooms.Count);

            foreach (var room in rooms)
            {
                room.WriteTo(PacketModified);
            }

            PacketModified.Append(false); // No official room.
        }
    }
}
