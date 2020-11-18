using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
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
    }, Outgoing.NavigatorSettings)]
    public class NavigatorSettingsModifier : PacketModifierBase
    {
        public NavigatorSettingsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int HomeRoomId { get; set; }
        public int HomeRoomId2 { get; set; } // ?

        public override void Parse()
        {
            if (Connection.LastIncomingPacket == Incoming.UpdateHomeRoom)
            {
                // We discard this packet because the habbo air client will
                // otherwise try to enter the room.
                Discard = true;
                return;
            }

            HomeRoomId = PacketOriginal.NextInt();
            HomeRoomId2 = PacketOriginal.NextInt();

            if (ReleaseFrom == Release.PRODUCTION_201701242205_837386174)
            {
                HomeRoomId = 0;
                HomeRoomId2 = 0;
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(HomeRoomId);
            PacketModified.Append(HomeRoomId2);
        }
    }
}
