using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using HabCrypto.Exceptions;
using HabCrypto.Interfaces;
using HabCrypto.Utils;

namespace HabCrypto
{
    // Note:
    // - C#     is LittleEndian     by default.
    // - AS3    is BigEndian        by default.
    //          always uses unsigned for biginteger blocks.
    public class HabboRSACrypto : IHabboRSACrypto
    {
        private readonly BigInteger _e;

        private readonly BigInteger _n;

        private readonly BigInteger _d;

        private readonly int _blockSize;

        public HabboRSACrypto(string e, string n)
        {
            _e = BigInteger.Parse('0' + e, NumberStyles.HexNumber);
            _n = BigInteger.Parse('0' + n, NumberStyles.HexNumber);
            _blockSize = (_n.BitLength() + 7) / 8;
        }

        public HabboRSACrypto(string e, string n, string d)
        {
            _e = BigInteger.Parse('0' + e, NumberStyles.HexNumber);
            _n = BigInteger.Parse('0' + n, NumberStyles.HexNumber);
            _d = BigInteger.Parse('0' + d, NumberStyles.HexNumber);
            _blockSize = (_n.BitLength() + 7) / 8;
        }

        public Span<byte> Encrypt(ReadOnlySpan<byte> data)
        {
            return DoEncrypt(DoPublic, data, 2);
        }

        public Span<byte> Decrypt(ReadOnlySpan<byte> data)
        {
            return DoDecrypt(DoPrivate, data, 2);
        }

        public Span<byte> Sign(ReadOnlySpan<byte> data)
        {
            return DoEncrypt(DoPrivate, data, 1);
        }

        public Span<byte> Verify(ReadOnlySpan<byte> data)
        {
            return DoDecrypt(DoPublic, data, 1);
        }

        private BigInteger DoPublic(BigInteger x)
        {
            return BigInteger.ModPow(x, _e, _n);
        }

        private BigInteger DoPrivate(BigInteger x)
        {
            return BigInteger.ModPow(x, _d, _n);
        }

        private Span<byte> DoEncrypt(Func<BigInteger, BigInteger> op, ReadOnlySpan<byte> data, int padType)
        {
            using (var dst = new MemoryStream())
            {
                var bl = _blockSize;
                var end = data.Length;
                var pos = 0;

                while (pos < end)
                {
                    var padded = Pkcs1Pad(data, ref pos, end, bl, padType);
                    var block = new BigInteger(padded, false, true);
                    var chunk = op(block);
                    
                    for (var b = bl - Math.Ceiling(chunk.BitLength() / 8.0); b > 0; --b)
                    {
                        dst.WriteByte(0x00);
                    }

                    dst.Write(chunk.ToByteArray(true, true));
                }

                return dst.ToArray();
            }
        }

        private Span<byte> DoDecrypt(Func<BigInteger, BigInteger> op, ReadOnlySpan<byte> data, int padType)
        {
            if (data.Length % _blockSize != 0)
            {
                throw new HabboCryptoException($"Decryption data was not in blocks of {_blockSize} bytes.");
            }

            using (var dst = new MemoryStream())
            {
                var bl = _blockSize;
                var end = data.Length;
                var pos = 0;

                while (pos < end)
                {
                    var block = new BigInteger(data.Slice(pos, bl), true, true);
                    var chunk = op(block);
                    var unpadded = Pkcs1Unpad(chunk.ToByteArray(false, true), bl, padType);

                    pos += bl;

                    dst.Write(unpadded);
                }

                return dst.ToArray();
            }
        }

        private static Span<byte> Pkcs1Pad(ReadOnlySpan<byte> src, ref int pos, int end, int n, int padType)
        {
            var result = new byte[n];

            var p = pos;
            end = Math.Min(end, Math.Min((int) src.Length, p + n - 11));
            pos = end;
            var i = end - 1;

            while (i >= p && n > 11)
            {
                result[--n] = src[i--];
            }

            result[--n] = 0;
            if (padType == 2)
            {
                var random = new Random();
                while (n > 2)
                {
                    result[--n] = (byte) random.Next(1, 256);
                }
            }
            else
            {
                while (n > 2)
                {
                    result[--n] = 0xFF;
                }
            }
            result[--n] = (byte) padType;
            result[--n] = 0;

            return result;
        }
        
        private static Span<byte> Pkcs1Unpad(ReadOnlySpan<byte> b, int n, int padType)
        {
            var result = new byte[n];
            var resultPos = 0;
            var i = 0;

            while (i < b.Length && b[i] == 0)
            {
                ++i;
            }

            if (b.Length - i != n - 1 || b[i] != padType)
            {
                throw new HabboCryptoException("PKCS#1 unpad: i=" + i + ", expected b[i]==" + padType + ", got b[i]=" + b[i].ToString("X2"));
            }

            ++i;

            while (b[i] != 0)
            {
                if (++i >= b.Length)
                {
                    throw new HabboCryptoException("PKCS#1 unpad: i=" + i + ", b[i-1]!=0 (=" + b[i].ToString("X2") + ")");
                }
            }

            while (++i < b.Length)
            {
                result[resultPos++] = b[i];
            }

            return result.AsSpan().Slice(0, resultPos);
        }
    }
}
