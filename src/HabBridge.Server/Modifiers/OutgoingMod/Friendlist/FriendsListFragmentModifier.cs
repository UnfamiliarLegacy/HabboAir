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
    }, Outgoing.FriendsListFragment)]
    public class FriendsListFragmentModifier : PacketModifierBase
    {
        public FriendsListFragmentModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Unknown0 { get; set; }

        public int Unknown1 { get; set; }

        public List<FriendData> Friends { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextInt();
            Friends = new List<FriendData>(PacketOriginal.NextInt());

            for (var i = 0; i < Friends.Capacity; i++)
            {
                Friends.Add(new FriendData(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Friends.Count);

            foreach (var friend in Friends)
            {
                PacketModified.Append(friend.Id);
                PacketModified.Append(friend.Username);
                PacketModified.Append(friend.Gender);
                PacketModified.Append(friend.Online);
                PacketModified.Append(friend.InRoom);
                PacketModified.Append(friend.Figure);
                PacketModified.Append(friend.CategoryId);
                PacketModified.Append(friend.Motto);
                PacketModified.Append(friend.FacebookUsername);
                PacketModified.Append(friend.Unknown1);
                PacketModified.Append(friend.AllowOfflineMessaging);
                PacketModified.Append(friend.Unknown3);
                PacketModified.Append(friend.IsMobileUser);
                PacketModified.Append(friend.RelationshipStatus);
            }
        }
    }
}
