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
    }, Outgoing.HabboSearchResult)]
    public class HabboSearchResultModifier : PacketModifierBase
    {
        public HabboSearchResultModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<HabboSearchResultData> Friends { get; set; }

        public List<HabboSearchResultData> Others { get; set; }

        public override void Parse()
        {
            Friends = new List<HabboSearchResultData>(PacketOriginal.NextInt());

            for (var i = 0; i < Friends.Capacity; i++)
            {
                Friends.Add(new HabboSearchResultData
                {
                    UserId = PacketOriginal.NextInt(),
                    Username = PacketOriginal.NextString(),
                    Motto = PacketOriginal.NextString(),
                    IsOnline = PacketOriginal.NextBool(),
                    Unknown0 = PacketOriginal.NextBool(),
                    Unknown1 = PacketOriginal.NextString(),
                    Unknown2 = PacketOriginal.NextInt(),
                    Figure = PacketOriginal.NextString(),
                    LastOnline = PacketOriginal.NextString()
                });
            }

            Others = new List<HabboSearchResultData>(PacketOriginal.NextInt());

            for (var i = 0; i < Others.Capacity; i++)
            {
                Others.Add(new HabboSearchResultData
                {
                    UserId = PacketOriginal.NextInt(),
                    Username = PacketOriginal.NextString(),
                    Motto = PacketOriginal.NextString(),
                    IsOnline = PacketOriginal.NextBool(),
                    Unknown0 = PacketOriginal.NextBool(),
                    Unknown1 = PacketOriginal.NextString(),
                    Unknown2 = PacketOriginal.NextInt(),
                    Figure = PacketOriginal.NextString(),
                    LastOnline = PacketOriginal.NextString()
                });
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Friends.Count);

            foreach (var friend in Friends)
            {
                PacketModified.Append(friend.UserId);
                PacketModified.Append(friend.Username);
                PacketModified.Append(friend.Motto);
                PacketModified.Append(friend.IsOnline);
                PacketModified.Append(friend.Unknown0);
                PacketModified.Append(friend.Unknown1);
                PacketModified.Append(friend.Unknown2);
                PacketModified.Append(friend.Figure);
                PacketModified.Append(friend.LastOnline);
            }

            PacketModified.Append(Others.Count);

            foreach (var other in Others)
            {
                PacketModified.Append(other.UserId);
                PacketModified.Append(other.Username);
                PacketModified.Append(other.Motto);
                PacketModified.Append(other.IsOnline);
                PacketModified.Append(other.Unknown0);
                PacketModified.Append(other.Unknown1);
                PacketModified.Append(other.Unknown2);
                PacketModified.Append(other.Figure);
                PacketModified.Append(other.LastOnline);
            }
        }
    }
}
