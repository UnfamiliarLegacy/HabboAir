using System.Collections.Generic;
using System.Linq;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Registers;

namespace HabBridge.Server.Net.Packet
{
    public class PacketOutgoing : PacketBase
    {
        public PacketOutgoing(Release release, short header) : base(PacketType.Outgoing, release, header)
        {
            HeaderType = Registar.HeadersOutgoing[release].FirstOrDefault(x => x.Value == Header).Key;
        }

        public PacketOutgoing(Release release, IReadOnlyList<byte> packet) : base(PacketType.Outgoing, release, packet)
        {
            HeaderType = Registar.HeadersOutgoing[release].FirstOrDefault(x => x.Value == Header).Key;
        }

        public Outgoing HeaderType { get; }
    }
}
