using System.Collections.Generic;

namespace HabBridge.Server.Net.Packet.Interfaces
{
    public interface IPacketReader
    {
        int Pointer { get; set; }

        int BytesLeft { get; }

        bool BytesAvailable { get; }

        bool AllowRead { get; }

        bool NextBool();

        short NextShort();

        int NextInt();

        string NextString();

        byte[] NextBytes();

        List<byte> GetPayloadLeft();
    }
}
