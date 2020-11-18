using Flazzy.ABC.AVM2;
using Flazzy.ABC.AVM2.Instructions;

namespace Flazzy.Extensions
{
    public static class ASCodeExtensions
    {
        public static int LastIndexOf(this ASCode code, int startIndex, OPCode op)
        {
            for (var i = startIndex; i >= 0; i--)
            {
                if (code[i].OP == op)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int LastIndexOf(this ASCode code, OPCode op)
        {
            for (var i = code.Count - 1; i >= 0; i--)
            {
                if (code[i].OP == op)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexOf(this ASCode code, int startIndex, OPCode op)
        {
            for (var i = startIndex; i < code.Count; i++)
            {
                if (code[i].OP == op)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
