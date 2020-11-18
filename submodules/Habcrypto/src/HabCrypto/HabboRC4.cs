using System;

namespace HabCrypto
{
    public class HabboRC4
    {
        private int _i;
        private int _j;
        private readonly int[] _table = new int[256];

        public HabboRC4(ReadOnlySpan<byte> key)
        {
            var length = key.Length;

            for (; _i < 256; ++_i)
            {
                _table[_i] = _i;
            }

            _i = 0;
            _j = 0;
            for (; _i < 256; ++_i)
            {
                _j = (_j + _table[_i] + key[_i % length]) % 256;
                Swap(_i, _j);
            }

            _i = 0;
            _j = 0;
        }

        public static void Init(ReadOnlySpan<byte> key, ref HabboRC4 client)
        {
            client = new HabboRC4(key);
        }

        private void Swap(int a, int b)
        {
            var num = _table[a];
            _table[a] = _table[b];
            _table[b] = num;
        }

        public void Parse(Memory<byte> bytes)
        {
            Parse(bytes, bytes.Length);
        }

        public void Parse(Memory<byte> bytes, int size)
        {
            var num = 0;
            var span = bytes.Span;

            for (var index1 = 0; index1 < size; ++index1)
            {
                _i = (_i + 1) % 256;
                _j = (_j + _table[_i]) % 256;
                Swap(_i, _j);

                var index2 = (_table[_i] + _table[_j]) % 256;
                span[num++] = (byte) (span[index1] ^ (uint) _table[index2]);
            }
        }
    }
}
