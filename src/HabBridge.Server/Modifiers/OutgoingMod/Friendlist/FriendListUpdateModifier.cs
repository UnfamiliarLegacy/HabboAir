using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Friendlist;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Friendlist
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.FriendListUpdate)]
    public class FriendListUpdateModifier : PacketModifierBase
    {
        public FriendListUpdateModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<FriendCategoryData> Categories { get; set; }

        public List<FriendDataUpdate> FriendUpdates { get; set; }

        public override void Parse()
        {
            Categories = new List<FriendCategoryData>(PacketOriginal.NextInt());

            for (var i = 0; i < Categories.Capacity; i++)
            {
                Categories.Add(new FriendCategoryData(PacketOriginal));
            }

            FriendUpdates = new List<FriendDataUpdate>(PacketOriginal.NextInt());

            for (var i = 0; i < FriendUpdates.Capacity; i++)
            {
                FriendUpdates.Add(new FriendDataUpdate(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Categories.Count);

            foreach (var category in Categories)
            {
                PacketModified.Append(category.Id);
                PacketModified.Append(category.Name);
            }

            PacketModified.Append(FriendUpdates.Count);

            foreach (var update in FriendUpdates)
            {
                PacketModified.Append(update.Type);

                if (update.Type == -1)
                {
                    PacketModified.Append(update.FriendId);
                }
                else
                {
                    PacketModified.Append(update.Data.Id);
                    PacketModified.Append(update.Data.Username);
                    PacketModified.Append(update.Data.Gender);
                    PacketModified.Append(update.Data.Online);
                    PacketModified.Append(update.Data.InRoom);
                    PacketModified.Append(update.Data.Figure);
                    PacketModified.Append(update.Data.CategoryId);
                    PacketModified.Append(update.Data.Motto);
                    PacketModified.Append(update.Data.FacebookUsername);
                    PacketModified.Append(update.Data.Unknown1);
                    PacketModified.Append(update.Data.AllowOfflineMessaging);
                    PacketModified.Append(update.Data.Unknown3);
                    PacketModified.Append(update.Data.IsMobileUser);
                    PacketModified.Append(update.Data.RelationshipStatus);
                }
            }
        }
    }
}
