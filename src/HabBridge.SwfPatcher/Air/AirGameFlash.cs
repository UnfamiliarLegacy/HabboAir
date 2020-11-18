using System;
using System.Collections.Generic;
using Flazzy;
using Flazzy.ABC;
using Flazzy.IO;
using Flazzy.Records;
using Flazzy.Tags;

namespace HabBridge.SwfPatcher.Air
{
    public class AirGameFlash : ShockwaveFlash
    {
        public AirGameFlash(string path) : base(path)
        {
            AbcFiles = new List<ABCFile>();
            AbcTagFiles = new Dictionary<DoABCTag, ABCFile>();
        }

        public List<ABCFile> AbcFiles { get; }

        public ABCFile AbcSplash => AbcFiles[AirConstants.AbcFileIndexSplash];

        public ABCFile AbcGame => AbcFiles[AirConstants.AbcFileIndexGame];

        public Dictionary<DoABCTag, ABCFile> AbcTagFiles { get; }

        public override void Disassemble(Action<TagItem> callback)
        {
            base.Disassemble(callback);

            if (AirConstants.AbcFiles != AbcFiles.Count)
            {
                throw new ApplicationException("AbcFiles.Count mismatch.");
            }
        }

        protected override void WriteTag(TagItem tag, FlashWriter output)
        {
            if (tag.Kind == TagKind.DoABC)
            {
                var abcTag = (DoABCTag) tag;
                abcTag.ABCData = AbcTagFiles[abcTag].ToArray();
            }

            base.WriteTag(tag, output);
        }

        protected override TagItem ReadTag(HeaderRecord header, FlashReader input)
        {
            var tag = base.ReadTag(header, input);

            if (tag.Kind == TagKind.DoABC)
            {
                var abcTag = (DoABCTag) tag;
                var abcFile = new ABCFile(abcTag.ABCData);

                AbcTagFiles.Add(abcTag, abcFile);
                AbcFiles.Add(abcFile);
            }

            return tag;
        }
    }
}
