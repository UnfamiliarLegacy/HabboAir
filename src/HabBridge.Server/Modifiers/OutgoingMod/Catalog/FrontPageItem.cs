using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Catalog
{
    public class FrontPageItem
    {
        public FrontPageItem(IPacketReader packet)
        {
            Position = packet.NextInt();
            Name = packet.NextString();
            Image = packet.NextString();
            Type = packet.NextInt();

            switch (Type)
            {
                case 0:
                case 2:
                    TypeStr = packet.NextString();
                    break;
                case 1:
                    TypeInt = packet.NextInt();
                    break;
            }

            ExpirySeconds = packet.NextInt();
        }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(Position);
            packet.Append(Name);
            packet.Append(Image);
            packet.Append(Type);

            switch (Type)
            {
                case 0:
                case 2:
                    packet.Append(TypeStr);
                    break;
                case 1:
                    packet.Append(TypeInt);
                    break;
            }

            packet.Append(ExpirySeconds);
        }

        public int Position { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int Type { get; set; }

        public string TypeStr { get; set; }

        public int TypeInt { get; set; }

        public int ExpirySeconds { get; set; }
    }
}
