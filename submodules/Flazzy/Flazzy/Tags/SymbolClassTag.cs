using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Flazzy.IO;
using Flazzy.Records;

namespace Flazzy.Tags
{
    public class SymbolClassTag : TagItem
    {
        public List<Tuple<ushort, string>> Entries { get; }

        public SymbolClassTag()
            : base(TagKind.SymbolClass)
        {
            Entries = new List<Tuple<ushort, string>>();
        }

        public SymbolClassTag(HeaderRecord header, FlashReader input)
            : base(header)
        {
            var symbolCount = input.ReadUInt16();

            Entries = new List<Tuple<ushort, string>>();

            for (var i = 0; i < symbolCount; i++)
            {
                var id = input.ReadUInt16();
                var name = input.ReadNullString();

                Entries.Add(new Tuple<ushort, string>(id, name));
            }
        }

        public ushort AddSymbol(string name)
        {
            var nextId = (ushort) (Entries.Max(x => x.Item1) + 1);
        
            Entries.Add(new Tuple<ushort, string>(nextId, name));
        
            return nextId;
        }

        public override int GetBodySize()
        {
            var size = 0;
            size += sizeof(ushort);
            size += (sizeof(ushort) * Entries.Count);

            foreach (var pair in Entries)
            {
                size += (Encoding.UTF8.GetByteCount(pair.Item2) + 1);
            }

            return size;
        }

        protected override void WriteBodyTo(FlashWriter output)
        {
            output.Write((ushort) Entries.Count);

            foreach (var pair in Entries)
            {
                output.Write(pair.Item1);
                output.WriteNullString(pair.Item2);
            }
        }
    }
}