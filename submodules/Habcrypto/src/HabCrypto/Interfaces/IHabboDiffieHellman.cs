using System;

namespace HabCrypto.Interfaces
{
    public interface IHabboDiffieHellman
    {
        string GetPublicKey();

        string GetSignedPrime();

        string GetSignedGenerator();

        void DoHandshake(string signedPrime, string signedGenerator);

        Span<byte> GetSharedKey(string publicKeyStr);
    }
}