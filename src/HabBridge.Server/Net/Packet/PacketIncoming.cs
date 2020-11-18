using System.Collections.Generic;
using System.Linq;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Registers;

namespace HabBridge.Server.Net.Packet
{
    public class PacketIncoming : PacketBase
    {
        public PacketIncoming(Release release, short header) : base(PacketType.Incoming, release, header)
        {
            HeaderType = Registar.HeadersIncoming[release].FirstOrDefault(x => x.Value == Header).Key;
        }

        public PacketIncoming(Release release, IReadOnlyList<byte> packet) : base(PacketType.Incoming, release, packet)
        {
            HeaderType = Registar.HeadersIncoming[release].FirstOrDefault(x => x.Value == Header).Key;
        }

        public Incoming HeaderType { get; }
    }
}
