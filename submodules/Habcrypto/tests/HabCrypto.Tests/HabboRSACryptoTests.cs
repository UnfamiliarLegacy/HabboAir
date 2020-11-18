using System.Text;
using HabCrypto.Utils;
using Xunit;

namespace HabCrypto.Tests
{
    public class HabboRSACryptoTests
    {
        private const int TestAmount = 100;

        private const string Message = "Hoi dit is een test.";

        [Fact]
        public void TestRSAClientToServer()
        {
            for (var i = 0; i < TestAmount; i++)
            {
                var client = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus);
                var server = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus, CryptoConstants.PrivateExponent);

                var data = client.Encrypt(Encoding.ASCII.GetBytes("Hoi dit is een test."));
                var decrypted = server.Decrypt(data);

                Assert.Equal(Message, Encoding.ASCII.GetString(decrypted));
            }
        }

        [Fact]
        public void TestRSAServerToClient()
        {
            for (var i = 0; i < TestAmount; i++)
            {
                var server = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus, CryptoConstants.PrivateExponent);
                var client = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus);

                var data = server.Sign(Encoding.ASCII.GetBytes("Hoi dit is een test."));
                var decrypted = client.Verify(data);

                Assert.Equal(Message, Encoding.ASCII.GetString(decrypted));
            }
        }

        [Fact]
        public void TestRSAClientToServerLong()
        {
            // Message was prepared with AS3Crypto library.
            var message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent at tempor odio. Nulla commodo enim ex, ac porta risus venenatis eu. Nam luctus placerat elit vitae posuere. Integer tristique viverra risus, eget bibendum augue tristique vitae. Etiam id tortor id nulla faucibus semper ultrices id ante. Aliquam vehicula nisl id semper tristique. Aliquam pulvinar pharetra nisi, id vulputate justo sollicitudin nec.";
            var messageBytes = HexUtils.HexStringToBytes("68d79fdb8c8827287fff8ad958db861ed6a6b3f5c81c9ac239838e81bc0cba5bcaa0381a61b8a77d0963d9007b66c54985c6a94d94f131d754ee1a57ecfbc68ac1e60f5a2d9ab571f80c13b1a71ab2f723892ac7433232de0afd02a36cf3cf8b384a2dbaf0320312c9308f4bc584163d1e1a2bddb108f3ac4c452754fd412def51da1309f750f5402113081385cd50ff6f6be7301adc80828311da4b4745fdd2dd0d2636147ab523293a3edce249d8abfefff9b4c9da6f537c2d18b9c3a0e1418beb12263a6c0125950fb5ff45188280b16e3fd9fe2efe8f25a1d112405a3d2d5a3f593001227a3cc128c9f1a59456e4e787451ca94c6ec9b45ff3da65bf11057a2510eadfaa5dbba48222ba597102764feac9895dbde25bee5aec574cc81a7c504579f7c830c84ed107cfdcddc1d1489f17f3a53ff33c11afaf6748215cbb09912994856453e44f35caed0335b0156ee7a1ab6fbd5dc688cda7274ffa2885a54672620bfd1408d2e73fc6936ea7c89bb1371cf1987c88033ffa5b3ccd205ef956acbad527a80e307a177a5b5916f7214ede2e0e31b9d5bc511ef41d1a2a610bb347a6a17cc42afead217cca4f058ad76c9b38dea89de048cc16953a367035eaafba842e4cc93dec35090f0b443241db048dc77d4b5cc11e3cf88743f38d7c2a6ef502aec8c419af707e41ea9349a2565dbb7f9d4ce36715c731549a2d62f1b4");

            var server = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus, CryptoConstants.PrivateExponent);

            var messageDecrypted = server.Decrypt(messageBytes);
            var messageOriginal = Encoding.UTF8.GetString(messageDecrypted);

            Assert.Equal(message, messageOriginal);
        }

        [Fact]
        public void TestRSAServerToClientLong()
        {
            // Message was prepared with AS3Crypto library.
            var message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent at tempor odio. Nulla commodo enim ex, ac porta risus venenatis eu. Nam luctus placerat elit vitae posuere. Integer tristique viverra risus, eget bibendum augue tristique vitae. Etiam id tortor id nulla faucibus semper ultrices id ante. Aliquam vehicula nisl id semper tristique. Aliquam pulvinar pharetra nisi, id vulputate justo sollicitudin nec.";

            var server = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus, CryptoConstants.PrivateExponent);
            var client = new HabboRSACrypto(CryptoConstants.Exponent, CryptoConstants.Modulus);

            var messageEncrypted = server.Sign(Encoding.UTF8.GetBytes(message));
            var messageDecrypted = client.Verify(messageEncrypted);
            var messageOriginal = Encoding.UTF8.GetString(messageDecrypted);

            Assert.Equal(message, messageOriginal);
        }
    }
}
