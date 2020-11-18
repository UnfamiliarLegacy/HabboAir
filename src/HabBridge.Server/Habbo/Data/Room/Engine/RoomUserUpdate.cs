using System.Globalization;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Room.Engine
{
    public class RoomUserUpdate
    {
        public RoomUserUpdate(IPacketReader packet)
        {
            VirtualId = packet.NextInt();
            X = packet.NextInt();
            Y = packet.NextInt();
            Z = double.Parse(packet.NextString(), CultureInfo.InvariantCulture);
            RotHead = packet.NextInt();
            RotBody = packet.NextInt();
            Status = packet.NextString();
        }

        public int VirtualId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public double Z { get; set; }

        public int RotHead { get; set; }

        public int RotBody { get; set; }

        public string Status { get; set; }
    }
}
