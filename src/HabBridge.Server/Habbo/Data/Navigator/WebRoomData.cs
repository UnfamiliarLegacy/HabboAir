using System;
using System.Collections.Generic;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Navigator
{
    public class WebRoomData
    {
        public WebRoomData(IPacketReader packet)
        {
            FlatId = packet.NextInt();
            RoomName = packet.NextString();
            OwnerId = packet.NextInt();
            OwnerName = packet.NextString();
            RoomAccess = packet.NextInt();
            UsersNow = packet.NextInt();
            UsersMax = packet.NextInt();
            RoomDescription = packet.NextString();
            TradeSettings = packet.NextInt();
            Score = packet.NextInt();
            Ranking = packet.NextInt();
            Category = packet.NextInt();

            Tags = new List<string>(packet.NextInt());
            for (var i = 0; i < Tags.Capacity; i++)
            {
                Tags.Add(packet.NextString());
            }

            RoomType = (WebRoomDataRoomType) packet.NextInt();

            if (RoomType.HasFlag(WebRoomDataRoomType.Official))
            {
                OfficialRoomPicReference = packet.NextString();
            }

            if (RoomType.HasFlag(WebRoomDataRoomType.Group))
            {
                GroupId = packet.NextInt();
                GroupName = packet.NextString();
                GroupBadge = packet.NextString();
            }

            if (RoomType.HasFlag(WebRoomDataRoomType.Promotion))
            {
                PromotionName = packet.NextString();
                PromotionDescription = packet.NextString();
                PromotionMinutesLeft = packet.NextInt();
            }
        }

        public int FlatId { get; set; }

        public string RoomName { get; set; }

        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        public int RoomAccess { get; set; }

        public int UsersNow { get; set; }

        public int UsersMax { get; set; }

        public string RoomDescription { get; set; }

        public int TradeSettings { get; set; }

        public int Score { get; set; }

        public int Ranking { get; set; }

        public int Category { get; set; }

        public List<string> Tags { get; set; }

        public WebRoomDataRoomType RoomType { get; set; }

        // Official data.
        public string OfficialRoomPicReference { get; set; }

        // Group data.
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string GroupBadge { get; set; }

        // Promotion data.
        public string PromotionName { get; set; }

        public string PromotionDescription { get; set; }

        public int PromotionMinutesLeft { get; set; }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(FlatId);
            packet.Append(RoomName);
            packet.Append(OwnerId);
            packet.Append(OwnerName);
            packet.Append(RoomAccess);
            packet.Append(UsersNow);
            packet.Append(UsersMax);
            packet.Append(RoomDescription);
            packet.Append(TradeSettings);
            packet.Append(Score);
            packet.Append(Ranking);
            packet.Append(Category);

            packet.Append(Tags.Count);
            foreach (var tag in Tags)
            {
                packet.Append(tag);
            }

            packet.Append((int) RoomType);

            if (RoomType.HasFlag(WebRoomDataRoomType.Official))
            {
                packet.Append(OfficialRoomPicReference);
            }

            if (RoomType.HasFlag(WebRoomDataRoomType.Group))
            {
                packet.Append(GroupId);
                packet.Append(GroupName);
                packet.Append(GroupBadge);
            }

            if (RoomType.HasFlag(WebRoomDataRoomType.Promotion))
            {
                packet.Append(PromotionName);
                packet.Append(PromotionDescription);
                packet.Append(PromotionMinutesLeft);
            }
        }
    }

    [Flags]
    public enum WebRoomDataRoomType
    {
        Default = 0,
        Official = 1,
        Group = 2,
        Promotion = 4,
        Private = 8,
        Pets = 16
    }
}