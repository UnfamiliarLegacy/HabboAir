using System.Collections.Generic;
using Flazzy;
using Flazzy.ABC;
using Flazzy.IO;
using Flazzy.Records;
using Flazzy.Tags;

namespace HabBridge.SwfDeobfuscator.Flash
{
    public class FlashFile : ShockwaveFlash
    {
        public FlashFile(string path) : base(path)
        {
            AbcFiles = new List<ABCFile>();
            AbcTagFiles = new Dictionary<DoABCTag, ABCFile>();
        }

        public List<ABCFile> AbcFiles { get; }

        public Dictionary<DoABCTag, ABCFile> AbcTagFiles { get; }

        protected override void WriteTag(TagItem tag, FlashWriter output)
        {
            if (tag.Kind == TagKind.DoABC)
            {
                var abcTag = (DoABCTag)tag;
                abcTag.ABCData = AbcTagFiles[abcTag].ToArray();
            }

            base.WriteTag(tag, output);
        }

        protected override TagItem ReadTag(HeaderRecord header, FlashReader input)
        {
            var tag = base.ReadTag(header, input);

            if (tag.Kind == TagKind.DoABC)
            {
                var abcTag = (DoABCTag)tag;
                var abcFile = new ABCFile(abcTag.ABCData);

                AbcTagFiles.Add(abcTag, abcFile);
                AbcFiles.Add(abcFile);
            }

            return tag;
        }
    }
}
