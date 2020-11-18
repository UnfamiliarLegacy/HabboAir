using System;
using System.Collections.Generic;
using System.Linq;
using Flazzy.ABC;
using HabBridge.SwfPatcher.Air.ClassLookup;

namespace HabBridge.SwfPatcher.Air
{
    public class AirClassLookup
    {
        private static readonly Dictionary<HabboClass, ILookup> Lookups;

        static AirClassLookup()
        {
            Lookups = new Dictionary<HabboClass, ILookup>
            {
                {HabboClass.IncomingMessages, new LookupIncomingMessages()},
                {HabboClass.HabboCommunicationManager, new LookupCommunicationManager()},
                {HabboClass.CommunicationUtils, new LookupCommunicationUtils()},
                {HabboClass.KeyObfuscator, new LookupKeyObfuscator()}
            };
        }

        private readonly AirGameFlash _flash;

        private readonly Dictionary<HabboClass, ASClass> _cache;

        public AirClassLookup(AirGameFlash flash)
        {
            _flash = flash;
            _cache = new Dictionary<HabboClass, ASClass>();
        }

        public ASClass Get(HabboClass clazz)
        {
            // Check if cache already contains this class.
            if (_cache.TryGetValue(clazz, out var cacheResult))
            {
                return cacheResult;
            }

            // Search for the class.
            var resultArr = Lookups[clazz].SearchClass(_flash).ToArray();
            if (resultArr.Length != 1)
            {
                throw new ApplicationException($"Found more (or less) than 1 of the class {clazz}.");
            }

            _cache.Add(clazz, resultArr[0]);

            return resultArr[0];
        }
    }
}
