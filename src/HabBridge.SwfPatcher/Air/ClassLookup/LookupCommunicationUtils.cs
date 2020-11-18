using System.Collections.Generic;
using System.Linq;
using Flazzy.ABC;

namespace HabBridge.SwfPatcher.Air.ClassLookup
{
    public class LookupCommunicationUtils : ILookup
    {
        public IEnumerable<ASClass> SearchClass(AirGameFlash flash)
        {
            var abc = flash.AbcGame;

            var indexOne = abc.Pool.Strings.IndexOf("environment");
            var indexTwo = abc.Pool.Strings.IndexOf("login");
            var indexThree = abc.Pool.Strings.IndexOf("userid");

            return flash.AbcGame.Classes
                .AsParallel()
                .Where(x => x != null &&
                            x.Traits.Count > 3 &&
                            x.Traits[0].ValueKind == ConstantKind.String &&
                            x.Traits[1].ValueKind == ConstantKind.String &&
                            x.Traits[2].ValueKind == ConstantKind.String &&
                            x.Traits[0].ValueIndex == indexOne &&
                            x.Traits[1].ValueIndex == indexTwo &&
                            x.Traits[2].ValueIndex == indexThree);
        }
    }
}
