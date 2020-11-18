using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;
using HabCrypto.Exceptions;

namespace HabCrypto.Utils
{
    internal static class PrimeUtils
    {
        // Random generator (thread safe)
        private static readonly ThreadLocal<Random> gen = new ThreadLocal<Random>(() => new Random());

        private static Random Gen => gen.Value;

        private static readonly ThreadLocal<RandomNumberGenerator> genCrypto = new ThreadLocal<RandomNumberGenerator>(RandomNumberGenerator.Create);

        private static RandomNumberGenerator GenCrypto => genCrypto.Value;

        public static BigInteger GeneratePseudoPrime(int bits, int witnesses = 10)
        {
            if (bits % 8 != 0)
            {
                throw new HabboCryptoException("We could not divide the bits by 8.");
            }

            var bytes = new byte[bits / 8];

            BigInteger result;

            do
            {
                GenCrypto.GetBytes(bytes);
                result = new BigInteger(bytes);
            } while (!result.IsProbablyPrime(witnesses));

            return result;
        }

        public static int BitLength(this BigInteger value)
        {
            var bitLength = 0;

            do
            {
                bitLength++;
                value /= 2;
            } while (value != 0);

            return bitLength;
        }

        public static bool IsProbablyPrime(this BigInteger value, int witnesses = 10)
        {
            if (value <= 1)
                return false;

            if (witnesses <= 0)
                witnesses = 10;

            var d = value - 1;
            var s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            var bytes = new byte[value.ToByteArray().LongLength];
            BigInteger a;

            for (var i = 0; i < witnesses; i++)
            {
                do
                {
                    Gen.NextBytes(bytes);

                    a = new BigInteger(bytes);
                } while (a < 2 || a >= value - 2);

                var x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1)
                    continue;

                for (var r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);

                    if (x == 1)
                        return false;
                    if (x == value - 1)
                        break;
                }

                if (x != value - 1)
                    return false;
            }

            return true;
        }
    }
}