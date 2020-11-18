using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
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
    }, Outgoing.ScrSendUserInfo)]
    public class ScrSendUserInfoModifier : PacketModifierBase
    {
        public ScrSendUserInfoModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string ProductName { get; set; }

        public int DisplayDays { get; set; }

        public int Unknown0 { get; set; }

        public int DisplayMonths { get; set; }

        public int Unknown1 { get; set; }

        public bool IsHabboClub { get; set; }

        public bool IsVip { get; set; }

        public int Unknown2 { get; set; }

        public int Unknown3 { get; set; }

        public int Unknown4 { get; set; }

        public int? Unknown5 { get; set; }

        public override void Parse()
        {
            ProductName = PacketOriginal.NextString();
            DisplayDays = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.NextInt();
            DisplayMonths = PacketOriginal.NextInt();
            Unknown1 = PacketOriginal.NextInt();
            IsHabboClub = PacketOriginal.NextBool();
            IsVip = PacketOriginal.NextBool();
            Unknown2 = PacketOriginal.NextInt();
            Unknown3 = PacketOriginal.NextInt();
            Unknown4 = PacketOriginal.NextInt();

            if (PacketOriginal.BytesAvailable)
            {
                Unknown5 = PacketOriginal.NextInt();
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(ProductName);
            PacketModified.Append(DisplayDays);
            PacketModified.Append(Unknown0);
            PacketModified.Append(DisplayMonths);
            PacketModified.Append(Unknown1);
            PacketModified.Append(IsHabboClub);
            PacketModified.Append(IsVip);
            PacketModified.Append(Unknown2);
            PacketModified.Append(Unknown3);
            PacketModified.Append(Unknown4);

            if (Unknown5.HasValue)
            {
                PacketModified.Append(Unknown5.Value);
            }
        }
    }
}
