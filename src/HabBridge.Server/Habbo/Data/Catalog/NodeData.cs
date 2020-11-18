using System.Collections.Generic;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Catalog
{
    public class NodeData
    {
        public NodeData(IPacketReader packet)
        {
            Visible = packet.NextBool();
            IconId = packet.NextInt();
            PageId = packet.NextInt();
            Unknown0 = packet.NextString();
            Localization = packet.NextString();

            Offers = new List<int>(packet.NextInt());
            for (var i = 0; i < Offers.Capacity; i++)
            {
                Offers.Add(packet.NextInt());
            }

            Children = new List<NodeData>(packet.NextInt());
            for (var i = 0; i < Children.Capacity; i++)
            {
                Children.Add(new NodeData(packet));
            }
        }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(Visible);
            packet.Append(IconId);
            packet.Append(PageId);
            packet.Append(Unknown0);
            packet.Append(Localization);

            packet.Append(Offers.Count);
            foreach (var offer in Offers)
            {
                packet.Append(offer);
            }

            packet.Append(Children.Count);
            foreach (var child in Children)
            {
                child.WriteTo(packet);
            }
        }

        public bool Visible { get; set; }

        public int IconId { get; set; }

        public int PageId { get; set; }

        public string Unknown0 { get; set; }

        public string Localization { get; set; }

        public List<int> Offers { get; set; }

        public List<NodeData> Children { get; set; }
    }
}
