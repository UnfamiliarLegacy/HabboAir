using System.Collections.Generic;
using System.Linq;
using Flazzy.ABC;

namespace HabBridge.SwfPatcher.Air.ClassLookup
{
    public class LookupKeyObfuscator : ILookup
    {
        public IEnumerable<ASClass> SearchClass(AirGameFlash flash)
        {
            return flash.AbcGame.Classes
                .AsParallel()
                .Where(clazz =>
                {
                    if (clazz == null)
                    {
                        return false;
                    }

                    if (clazz.Traits.Count(trait => trait.Kind == TraitKind.Slot) != 1)
                    {
                        return false;
                    }

                    var staticVariable = clazz.Traits.First(x => x.Kind == TraitKind.Slot);
                    if (staticVariable.QName.Namespace.Name.Contains("KeyObfuscator"))
                    {
                        return true;
                    }

                    return false;
                });
        }
    }
}
