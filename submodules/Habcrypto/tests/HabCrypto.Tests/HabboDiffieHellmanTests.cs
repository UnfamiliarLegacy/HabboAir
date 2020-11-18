using Xunit;

namespace HabCrypto.Tests
{
    public class HabboDiffieHellmanTests
    {
        [Fact]
        public void TestFullHandshake()
        {
            for (var i = 0; i < 25; i++)
            {
                var serverCrypto = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus, CryptoConstants.PrivateExponent);
                var server = new HabboDiffieHellman(serverCrypto, HabboConnectionType.Server);

                var clientCrypto = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus);
                var client = new HabboDiffieHellman(clientCrypto, HabboConnectionType.Client);

                // Server sends prime and generator to the client.
                client.DoHandshake(server.GetSignedPrime(), server.GetSignedGenerator());

                Assert.Equal(client.DHPrime, server.DHPrime);
                Assert.Equal(client.DHGenerator, server.DHGenerator);

                // Client sends DH public key to the server.
                var clientPublic = client.GetPublicKey();
                var serverKey = server.GetSharedKey(clientPublic);

                // Server sends DH public key to the client.
                var serverPublic = server.GetPublicKey();
                var clientKey = client.GetSharedKey(serverPublic);

                Assert.Equal(serverKey.ToArray(), clientKey.ToArray());
            }
        }
    }
}
