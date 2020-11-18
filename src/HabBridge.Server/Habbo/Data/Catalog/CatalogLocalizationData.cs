using System.Collections.Generic;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Catalog
{
    public class CatalogLocalizationData
    {
        public CatalogLocalizationData(IPacketReader packet)
        {
            One = new List<string>(packet.NextInt());
            for (var i = 0; i < One.Capacity; i++)
            {
                One.Add(packet.NextString());
            }

            Two = new List<string>(packet.NextInt());
            for (var i = 0; i < Two.Capacity; i++)
            {
                Two.Add(packet.NextString());
            }
        }

        public void WriteTo(IPacketWriter packet)
        {
            packet.Append(One.Count);
            foreach (var s in One)
            {
                packet.Append(s);
            }

            packet.Append(Two.Count);
            foreach (var s in Two)
            {
                packet.Append(s);
            }
        }

        public List<string> One { get; set; }

        public List<string> Two { get; set; }
    }
}
