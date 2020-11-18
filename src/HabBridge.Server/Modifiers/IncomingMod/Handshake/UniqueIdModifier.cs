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
    }, Incoming.UniqueId)]
    public class UniqueIdModifier : PacketModifierBase
    {
        public UniqueIdModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public string MachineId { get; set; }

        public string DeviceFingerprint { get; set; }

        public string Capabilities { get; set; }

        public override void Parse()
        {
            switch (ReleaseFrom)
            {
                case Release.AIR63_201805250931_867387450:
                case Release.AIR63_201911271159_623255659:
                    MachineId = PacketOriginal.NextString();
                    DeviceFingerprint = PacketOriginal.BytesLeft > 0 ? PacketOriginal.NextString() : string.Empty;
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
                    PacketModified.Append(MachineId);
                    PacketModified.Append(DeviceFingerprint);
                    PacketModified.Append("WIN/26,0,0,151");
                    break;
                case Release.PRODUCTION_201701242205_837386174:
                    PacketModified.Append(MachineId);
                    // Leet is kinda weird and needs a fingerprint..
                    PacketModified.Append(MachineId);
                    // PacketModified.Append(DeviceFingerprint);
                    PacketModified.Append("WIN/31,0,0,122");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ReleaseTarget), "Release was not added.");
            }
        }
    }
}
