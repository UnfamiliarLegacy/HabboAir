using System.Collections.Generic;
using Flazzy.ABC;

namespace HabBridge.SwfPatcher.Air.ClassLookup
{
    public interface ILookup
    {
        IEnumerable<ASClass> SearchClass(AirGameFlash flash);
    }
}
