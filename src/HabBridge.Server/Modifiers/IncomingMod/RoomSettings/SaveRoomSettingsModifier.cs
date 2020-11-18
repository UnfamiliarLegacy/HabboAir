using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.RoomSettings;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.RoomSettings
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.SaveRoomSettings)]
    public class SaveRoomSettingsModifier : PacketModifierBase
    {
        public SaveRoomSettingsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int RoomId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DoorMode { get; set; }

        public string Password { get; set; }

        public int MaxUsers { get; set; }

        public int CategoryId { get; set; }

        public List<string> Tags { get; set; }

        public int TradeSettings { get; set; }

        public bool AllowPets { get; set; }

        public bool AllowPetsEat { get; set; }

        public bool RoomBlockingEnabled { get; set; }

        public bool HideWall { get; set; }

        public int WallThickness { get; set; }

        public int FloorThickness { get; set; }

        public RoomModerationSettings ModerationSettings { get; set; }

        public RoomChatSettings ChatSettings { get; set; }

        public bool Unknown0 { get; set; }

        public override void Parse()
        {
            RoomId = PacketOriginal.NextInt();
            Name = PacketOriginal.NextString();
            Description = PacketOriginal.NextString();
            DoorMode = PacketOriginal.NextInt();
            Password = PacketOriginal.NextString();
            MaxUsers = PacketOriginal.NextInt();
            CategoryId = PacketOriginal.NextInt();

            Tags = new List<string>(PacketOriginal.NextInt());
            for (var i = 0; i < Tags.Capacity; i++)
            {
                Tags.Add(PacketOriginal.NextString());
            }

            TradeSettings = PacketOriginal.NextInt();
            AllowPets = PacketOriginal.NextBool();
            AllowPetsEat = PacketOriginal.NextBool();
            RoomBlockingEnabled = PacketOriginal.NextBool();
            HideWall = PacketOriginal.NextBool();
            WallThickness = PacketOriginal.NextInt();
            FloorThickness = PacketOriginal.NextInt();
            ModerationSettings = new RoomModerationSettings(PacketOriginal);
            ChatSettings = new RoomChatSettings(PacketOriginal);
            Unknown0 = PacketOriginal.NextBool();
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomId);
            PacketModified.Append(Name);
            PacketModified.Append(Description);
            PacketModified.Append(DoorMode);
            PacketModified.Append(Password);
            PacketModified.Append(MaxUsers);
            PacketModified.Append(CategoryId);

            PacketModified.Append(Tags.Count);
            foreach (var tag in Tags)
            {
                PacketModified.Append(tag);
            }

            PacketModified.Append(TradeSettings);
            PacketModified.Append(AllowPets);
            PacketModified.Append(AllowPetsEat);
            PacketModified.Append(RoomBlockingEnabled);
            PacketModified.Append(HideWall);
            PacketModified.Append(WallThickness);
            PacketModified.Append(FloorThickness);
            ModerationSettings.WriteTo(PacketModified);
            ChatSettings.WriteTo(PacketModified);
            PacketModified.Append(Unknown0);
        }
    }
}
