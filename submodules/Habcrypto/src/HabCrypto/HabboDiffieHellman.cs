using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using HabCrypto.Exceptions;
using HabCrypto.Interfaces;
using HabCrypto.Utils;

namespace HabCrypto
{
    public class HabboDiffieHellman : IHabboDiffieHellman
    {
        private const int DiffiePrimesBitSize = 256;

        private const int DiffieKeyBitSize = 256;

        private const int DiffieWitnesses = 20;

        public HabboDiffieHellman(HabboRSACrypto crypto, HabboConnectionType type)
        {
            Crypto = crypto;
            Type = type;

            if (Type == HabboConnectionType.Server)
            {
                GenerateDHPrimes();
                GenerateDHKeys();
            }
        }

        public HabboRSACrypto Crypto { get; }

        public HabboConnectionType Type { get; }

        // p
        public BigInteger DHPrime { get; set; }

        // g
        public BigInteger DHGenerator { get; set; }

        // a
        public BigInteger DHPrivate { get; set; }

        // g^a mod p
        public BigInteger DHPublic { get; set; }

        private void GenerateDHPrimes()
        {
            DHPrime = PrimeUtils.GeneratePseudoPrime(DiffiePrimesBitSize, DiffieWitnesses);
            DHGenerator = PrimeUtils.GeneratePseudoPrime(DiffiePrimesBitSize, DiffieWitnesses);
            
            // Prime needs to be bigger than the generator.
            if (DHGenerator > DHPrime)
            {
                var temp = DHPrime;

                DHPrime = DHGenerator;
                DHGenerator = temp;
            }
        }

        private void GenerateDHKeys()
        {
            DHPrivate = PrimeUtils.GeneratePseudoPrime(DiffieKeyBitSize, DiffieWitnesses);
            DHPublic = BigInteger.ModPow(DHGenerator, DHPrivate, DHPrime);
        }

        private string EncryptBigInteger(BigInteger integer)
        {
            var str = integer.ToString();
            var bytes = Encoding.UTF8.GetBytes(str);
            var encrypted = Type == HabboConnectionType.Server
                ? Crypto.Sign(bytes)
                : Crypto.Encrypt(bytes);

            return HexUtils.BytesToHexString(encrypted);
        }

        private BigInteger DecryptBigInteger(string str)
        {
            // TODO: Remove Leet hotfix.
            if (str.Length == 255)
            {
                str = '0' + str;
            }
            
            var bytes = HexUtils.HexStringToBytes(str);
            var decrypted = Type == HabboConnectionType.Server
                ? Crypto.Decrypt(bytes)
                : Crypto.Verify(bytes);
            var intStr = Encoding.UTF8.GetString(decrypted);

            return BigInteger.Parse(intStr, NumberStyles.None);
        }

        public string GetPublicKey()
        {
            return EncryptBigInteger(DHPublic);
        }

        public string GetSignedPrime()
        {
            return EncryptBigInteger(DHPrime);
        }

        public string GetSignedGenerator()
        {
            return EncryptBigInteger(DHGenerator);
        }

        public void DoHandshake(string signedPrime, string signedGenerator)
        {
            DHPrime = DecryptBigInteger(signedPrime);
            DHGenerator = DecryptBigInteger(signedGenerator);

            if (DHPrime <= 2)
            {
                throw new HabboCryptoException("Prime cannot be <= 2!\nPrime: " + DHPrime);
            }

            if (DHGenerator >= DHPrime)
            {
                throw new HabboCryptoException($"Generator cannot be >= Prime!\nPrime: {DHPrime}\nGenerator: {DHGenerator}");
            }

            GenerateDHKeys();
        }

        public Span<byte> GetSharedKey(string publicKeyStr)
        {
            var publicKey = DecryptBigInteger(publicKeyStr);
            var sharedKey = BigInteger.ModPow(publicKey, DHPrivate, DHPrime);

            return sharedKey.ToByteArray(true, true);
        }
    }
}
