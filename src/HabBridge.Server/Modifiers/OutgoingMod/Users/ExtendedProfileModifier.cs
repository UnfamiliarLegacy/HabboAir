using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Users;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Users
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.ExtendedProfile)]
    public class ExtendedProfileModifier : PacketModifierBase
    {
        public ExtendedProfileModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Figure { get; set; }

        public string Motto { get; set; }

        public string CreationDate { get; set; }

        public int AchievementPoints { get; set; }

        public int FriendCount { get; set; }

        public bool IsFriend { get; set; }

        public bool IsFriendRequestPending { get; set; }

        public bool IsOnline { get; set; }

        public List<HabboGroupEntryData> Groups { get; set; }

        public int LastOnline { get; set; }

        public bool ShowProfile { get; set; }

        public override void Parse()
        {
            UserId = PacketOriginal.NextInt();
            Username = PacketOriginal.NextString();
            Figure = PacketOriginal.NextString();
            Motto = PacketOriginal.NextString();
            CreationDate = PacketOriginal.NextString();
            AchievementPoints = PacketOriginal.NextInt();
            FriendCount = PacketOriginal.NextInt();
            IsFriend = PacketOriginal.NextBool();
            IsFriendRequestPending = PacketOriginal.NextBool();
            IsOnline = PacketOriginal.NextBool();

            Groups = new List<HabboGroupEntryData>(PacketOriginal.NextInt());

            for (var i = 0; i < Groups.Capacity; i++)
            {
                Groups.Add(new HabboGroupEntryData
                {
                    GroupId = PacketOriginal.NextInt(),
                    Name = PacketOriginal.NextString(),
                    Badge = PacketOriginal.NextString(),
                    Colour1 = PacketOriginal.NextString(),
                    Colour2 = PacketOriginal.NextString(),
                    IsFavourite = PacketOriginal.NextBool(),
                    Unknown0 = PacketOriginal.NextInt(),
                    IsForumEnabled = PacketOriginal.NextBool()
                });   
            }

            LastOnline = PacketOriginal.NextInt();
            ShowProfile = PacketOriginal.NextBool();
        }

        public override void Recreate()
        {
            PacketModified.Append(UserId);
            PacketModified.Append(Username);
            PacketModified.Append(Figure);
            PacketModified.Append(Motto);
            PacketModified.Append(CreationDate);
            PacketModified.Append(AchievementPoints);
            PacketModified.Append(FriendCount);
            PacketModified.Append(IsFriend);
            PacketModified.Append(IsFriendRequestPending);
            PacketModified.Append(IsOnline);
            PacketModified.Append(Groups.Count);

            foreach (var group in Groups)
            {
                PacketModified.Append(group.GroupId);
                PacketModified.Append(group.Name);
                PacketModified.Append(group.Badge);
                PacketModified.Append(group.Colour1);
                PacketModified.Append(group.Colour2);
                PacketModified.Append(group.IsFavourite);
                PacketModified.Append(group.Unknown0);
                PacketModified.Append(group.IsForumEnabled);
            }

            PacketModified.Append(LastOnline);
            PacketModified.Append(ShowProfile);
        }
    }
}
