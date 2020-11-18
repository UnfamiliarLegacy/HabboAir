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
    }, Outgoing.NewFriendRequest)]
    public class NewFriendRequestModifier : PacketModifierBase
    {
        public NewFriendRequestModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public FriendRequestData FriendRequest { get; set; }

        public override void Parse()
        {
            FriendRequest = new FriendRequestData(PacketOriginal);
        }

        public override void Recreate()
        {
            PacketModified.Append(FriendRequest.UserId);
            PacketModified.Append(FriendRequest.Username);
            PacketModified.Append(FriendRequest.Figure);
        }
    }
}
