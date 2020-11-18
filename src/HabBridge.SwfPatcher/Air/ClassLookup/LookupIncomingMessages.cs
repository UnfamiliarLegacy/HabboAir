using System.Collections.Generic;
using System.Linq;
using Flazzy.ABC;

namespace HabBridge.SwfPatcher.Air.ClassLookup
{
    public class LookupIncomingMessages : ILookup
    {
        public IEnumerable<ASClass> SearchClass(AirGameFlash flash)
        {
            return flash.AbcGame.Classes
                .AsParallel()
                .Where(x => x?.Instance != null &&
                            x.Instance.IsInterface == false &&
                            x.Instance.GetMethod("handleWebLogout") != null);
        }
    }
}
