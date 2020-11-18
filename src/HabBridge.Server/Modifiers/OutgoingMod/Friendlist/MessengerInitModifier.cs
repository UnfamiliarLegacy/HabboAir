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
    }, Outgoing.MessengerInit)]
    public class MessengerInitModifier : PacketModifierBase
    {
        public MessengerInitModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int FriendsMax { get; set; }

        public int Unknown0 { get; set; }

        public int Unknown1 { get; set; }

        public List<FriendCategoryData> Categories { get; set; }

        public override void Parse()
        {
            FriendsMax = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextInt();
            Categories = new List<FriendCategoryData>(PacketOriginal.NextInt());

            for (var i = 0; i < Categories.Capacity; i++)
            {
                Categories.Add(new FriendCategoryData(PacketOriginal));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(FriendsMax);
            PacketModified.Append(Unknown0);
            PacketModified.Append(Unknown1);
            PacketModified.Append(Categories.Count);

            foreach (var category in Categories)
            {
                PacketModified.Append(category.Id);
                PacketModified.Append(category.Name);
            }
        }
    }
}
