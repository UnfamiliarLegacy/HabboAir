using System;
using System.Security.Cryptography;
using System.Text;

namespace HabBridge.Api.Services.Sessions
{
    public class SessionId
    {
        public Guid Id { get; set; }

        public long Timestamp { get; set; }

        public string Hash { get; set; }

        public void Init()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Hash = GetHash(Id, Timestamp);
        }

        private static string GetHash(Guid id, long timestamp)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Generate random bytes.
                var bytesJunk = new byte[32];
                rng.GetBytes(bytesJunk);

                // Create a hash.
                using (var hash = new SHA256Managed())
                {
                    var bytesStr = Encoding.ASCII.GetBytes($"{id.ToString()}_{timestamp}");
                    var bytesInput = new byte[bytesJunk.Length + bytesStr.Length];

                    Buffer.BlockCopy(bytesJunk, 0, bytesInput, 0, bytesJunk.Length);
                    Buffer.BlockCopy(bytesStr, 0, bytesInput, bytesJunk.Length, bytesStr.Length);

                    var bytes = hash.ComputeHash(bytesInput);

                    return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
                }
            }
        }
    }
}
