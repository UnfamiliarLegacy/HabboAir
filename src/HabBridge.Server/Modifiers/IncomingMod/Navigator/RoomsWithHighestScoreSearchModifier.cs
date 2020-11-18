using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Session.Navigator;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Navigator
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.RoomsWithHighestScoreSearch)]
    public class RoomsWithHighestScoreSearchModifier : PacketModifierBase
    {
        private const string Category = "myworld_view";

        private const string CategoryId = "highest_score";

        public RoomsWithHighestScoreSearchModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public override void Parse()
        {
            PacketOriginal.NextInt();

            Connection.Session.Navigator.SetLastSearch(Category, CategoryId, NavigatorSearchType.MyWorldViewHighestScore);
        }

        public override void Recreate()
        {
            PacketModified.Append(Connection.Session.Navigator.LastCategorySearch);
            PacketModified.Append(string.Empty);
        }
    }
}
