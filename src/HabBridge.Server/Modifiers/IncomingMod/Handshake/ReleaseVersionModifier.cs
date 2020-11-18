using System;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.IncomingMod.Handshake
{
    [DefineIncomingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Incoming.ReleaseVersion)]
    public class ReleaseVersionModifier : PacketModifierBase
    {
        public ReleaseVersionModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public string HotelType { get; set; }

        public override void Parse()
        {
            switch (ReleaseFrom)
            {
                case Release.AIR63_201805250931_867387450:
                case Release.AIR63_201911271159_623255659:
                    PacketOriginal.NextString(); // Release version
                    PacketOriginal.NextString(); // Protocol
                    Unknown1 = PacketOriginal.NextInt();
                    Unknown2 = PacketOriginal.NextInt();
                    HotelType = PacketOriginal.BytesAvailable ? PacketOriginal.NextString() : null; // HotelType (Patched SWF)
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Recreate()
        {
            switch (ReleaseTarget)
            {
                case Release.PRODUCTION_201607262204_86871104:
                    PacketModified.Append("PRODUCTION-201607262204-86871104");
                    PacketModified.Append("FLASH");
                    PacketModified.Append(1); // Constant value
                    PacketModified.Append(0); // Constant value
                    break;
                case Release.PRODUCTION_201701242205_837386174:
                    PacketModified.Append("PRODUCTION-201701242205-837386174");
                    PacketModified.Append("FLASH");
                    PacketModified.Append(1); // Constant value
                    PacketModified.Append(0); // Constant value
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ReleaseTarget), "Release was not added.");
            }
        }
    }
}
