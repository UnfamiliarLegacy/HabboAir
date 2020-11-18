using System;

namespace HabCrypto.Exceptions
{
    public class HabboCryptoException : Exception
    {
        public HabboCryptoException(string message) : base(message)
        {
        }

        public HabboCryptoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
