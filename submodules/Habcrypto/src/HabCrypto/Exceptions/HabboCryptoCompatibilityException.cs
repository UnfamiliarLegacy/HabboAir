using System;

namespace HabCrypto.Exceptions
{
    public class HabboCryptoCompatibilityException : HabboCryptoException
    {
        public HabboCryptoCompatibilityException(string message) : base(message)
        {
        }

        public HabboCryptoCompatibilityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
