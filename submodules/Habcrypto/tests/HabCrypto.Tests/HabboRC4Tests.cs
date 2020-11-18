using System.Text;
using Xunit;

namespace HabCrypto.Tests
{
    public class HabboRC4Tests
    {
        private static readonly byte[] Key = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F};

        private const string Message = "Hi, this is a test.";

        [Fact]
        public void TestEncryption()
        {
            var server = new HabboRC4(Key);
            var client = new HabboRC4(Key);

            var message = Encoding.ASCII.GetBytes(Message);

            server.Parse(message);
            client.Parse(message);
            
            var decrypted = Encoding.ASCII.GetString(message);

            Assert.Equal(Message, decrypted);
        }
    }
}
