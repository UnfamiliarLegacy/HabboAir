using System.Collections.Generic;
using HabBridge.Habbo.Shared;

namespace HabBridge.Server.Net.Packet.Interfaces
{
    public interface IPacket : IPacketReader, IPacketWriter
    {
        PacketType Type { get; }

        Release Release { get; }

        short Header { get; }

        int Length { get; }

        List<byte> Payload { get; }
    }
}
