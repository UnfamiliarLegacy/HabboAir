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
    }, Outgoing.FriendRequests)]
    public class FriendRequestsModifier : PacketModifierBase
    {
        public FriendRequestsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int TotalRequests { get; set; }

        public List<FriendRequestData> Requests { get; set; }

        public override void Parse()
        {
            TotalRequests = PacketOriginal.NextInt();
            Requests = new List<FriendRequestData>(PacketOriginal.NextInt());

            for (var i = 0; i < Requests.Capacity; i++)
            {
                Requests.Add(new FriendRequestData(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(TotalRequests);
            PacketModified.Append(Requests.Count);

            foreach (var request in Requests)
            {
                PacketModified.Append(request.UserId);
                PacketModified.Append(request.Username);
                PacketModified.Append(request.Figure);
            }
        }
    }
}
