using System;

namespace HabCrypto.Interfaces
{
    public interface IHabboRSACrypto
    {
        Span<byte> Encrypt(ReadOnlySpan<byte> data);

        Span<byte> Decrypt(ReadOnlySpan<byte> data);

        Span<byte> Sign(ReadOnlySpan<byte> data);

        Span<byte> Verify(ReadOnlySpan<byte> data);
    }
}
