using System.Collections.Generic;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Navigator
{
    public class WebSearchResultList
    {
        public WebSearchResultList(IPacketReader packet)
        {
            CategoryId = packet.NextString();
            PublicName = packet.NextString();
            ActionAllowed = packet.NextInt();
            IsMinimized = packet.NextBool();
            ViewMode = (WebSearchResultListViewMode) packet.NextInt();
            GuestRooms = new List<WebRoomData>(packet.NextInt());

            for (var i = 0; i < GuestRooms.Capacity; i++)
            {
                GuestRooms.Add(new WebRoomData(packet));
            }
        }

        public string CategoryId { get; set; }

        public string PublicName { get; set; }

        public int ActionAllowed { get; set; }

        public bool IsMinimized { get; set; }

        public WebSearchResultListViewMode ViewMode { get; set; }

        public List<WebRoomData> GuestRooms { get; set; }
    }

    public enum WebSearchResultListViewMode
    {
        Regular,
        Thumbnail
    }
}
