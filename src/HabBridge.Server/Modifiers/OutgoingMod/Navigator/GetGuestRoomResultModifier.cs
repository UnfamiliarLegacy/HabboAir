using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Navigator;
using HabBridge.Server.Habbo.Data.RoomSettings;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Navigator
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.GetGuestRoomResult)]
    public class GetGuestRoomResultModifier : PacketModifierBase
    {
        public GetGuestRoomResultModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public bool Unknown0 { get; set; }

        public WebRoomData RoomData { get; set; }

        public bool Unknown1 { get; set; }

        public bool Unknown2 { get; set; }

        public bool Unknown3 { get; set; }

        public bool Unknown4 { get; set; }

        public RoomModerationSettings ModerationSettings { get; set; }

        public bool Unknown5 { get; set; }

        public RoomChatSettings ChatSettings { get; set; }

        public override void Parse()
        {
            Unknown0 = PacketOriginal.NextBool();
            RoomData = new WebRoomData(PacketOriginal);
            Unknown1 = PacketOriginal.NextBool();
            Unknown2 = PacketOriginal.NextBool();
            Unknown3 = PacketOriginal.NextBool();
            Unknown4 = PacketOriginal.NextBool();
            ModerationSettings = new RoomModerationSettings(PacketOriginal);
            Unknown5 = PacketOriginal.NextBool();
            ChatSettings = new RoomChatSettings(PacketOriginal);
        }

        public override void Recreate()
        {
            PacketModified.Append(Unknown0);

            RoomData.WriteTo(PacketModified);

            PacketModified.Append(Unknown1);
            PacketModified.Append(Unknown2);
            PacketModified.Append(Unknown3);
            PacketModified.Append(Unknown4);

            ModerationSettings.WriteTo(PacketModified);

            PacketModified.Append(Unknown5);

            ChatSettings.WriteTo(PacketModified);
        }
    }
}
