using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.RoomSettings;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.RoomSettings
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.RoomSettingsData)]
    public class RoomSettingsDataModifier : PacketModifierBase
    {
        public RoomSettingsDataModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int RoomId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DoorMode { get; set; }

        public int CategoryId { get; set; }

        public int MaxUsers { get; set; }

        public int Unknown0 { get; set; }

        public List<string> Tags { get; set; }

        public int TradeSettings { get; set; }

        public int AllowPets { get; set; }

        public int AllowPetsEat { get; set; }

        public int RoomBlockingEnabled { get; set; }

        public int HideWall { get; set; }

        public int WallThickness { get; set; }

        public int FloorThickness { get; set; }

        public RoomChatSettings ChatSettings { get; set; }

        public bool Unknown1 { get; set; }

        public RoomModerationSettings ModerationSettings { get; set; }

        public override void Parse()
        {
            RoomId = PacketOriginal.NextInt();
            Name = PacketOriginal.NextString();
            Description = PacketOriginal.NextString();
            DoorMode = PacketOriginal.NextInt();
            CategoryId = PacketOriginal.NextInt();
            MaxUsers = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.NextInt();

            Tags = new List<string>(PacketOriginal.NextInt());
            for (var i = 0; i < Tags.Capacity; i++)
            {
                Tags.Add(PacketOriginal.NextString());
            }

            TradeSettings = PacketOriginal.NextInt();
            AllowPets = PacketOriginal.NextInt();
            AllowPetsEat = PacketOriginal.NextInt();
            RoomBlockingEnabled = PacketOriginal.NextInt();
            HideWall = PacketOriginal.NextInt();
            WallThickness = PacketOriginal.NextInt();
            FloorThickness = PacketOriginal.NextInt();
            ModerationSettings = new RoomModerationSettings(PacketOriginal);
            Unknown1 = PacketOriginal.NextBool();
            ChatSettings = new RoomChatSettings(PacketOriginal);
        }

        public override void Recreate()
        {
            PacketModified.Append(RoomId);
            PacketModified.Append(Name);
            PacketModified.Append(Description);
            PacketModified.Append(DoorMode);
            PacketModified.Append(CategoryId);
            PacketModified.Append(MaxUsers);
            PacketModified.Append(Unknown0);

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

            ChatSettings.WriteTo(PacketModified);

            PacketModified.Append(Unknown1);

            ModerationSettings.WriteTo(PacketModified);
        }
    }
}
