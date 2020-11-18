using System.Collections.Generic;

namespace HabBridge.Server.Net.Packet.Interfaces
{
    public interface IPacketWriter
    {
        bool AllowWrite { get; }

        IPacketWriter Append(bool value);

        IPacketWriter Append(short value);

        IPacketWriter Append(int value);

        IPacketWriter Append(string value);

        IPacketWriter Append(IEnumerable<byte> bytes);

        List<byte> GetPayload();

        List<byte> GetPacket();
    }
}
