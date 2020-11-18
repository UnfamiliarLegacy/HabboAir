using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Room.Engine
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.UserChange)]
    public class UserChangeModifier : PacketModifierBase
    {
        public UserChangeModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int VirtualId { get; set; }

        public string Figure { get; set; }

        public string Gender { get; set; }

        public string Motto { get; set; }

        public int AchievementPoints { get; set; }

        public override void Parse()
        {
            VirtualId = PacketOriginal.NextInt();
            Figure = PacketOriginal.NextString();
            Gender = PacketOriginal.NextString();
            Motto = PacketOriginal.NextString();
            AchievementPoints = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(VirtualId);
            PacketModified.Append(Figure);
            PacketModified.Append(Gender);
            PacketModified.Append(Motto);
            PacketModified.Append(AchievementPoints);
        }
    }
}
