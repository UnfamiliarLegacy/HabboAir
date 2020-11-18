namespace HabBridge.Server.Net.Encoders
{
    public class ByteEncoding
    {
        public static byte[] EncodeInt(int value)
        {
            var result = new byte[4];

            result[0] = (byte) (value >> 24);
            result[1] = (byte) (value >> 16);
            result[2] = (byte) (value >> 8);
            result[3] = (byte) value;

            return result;
        }

        public static int DecodeInt(byte[] value)
        {
            if ((value[0] | value[1] | value[2] | value[3]) < 0)
            {
                return -1;
            }

            return (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
        }

        public static byte[] EncodeShort(short value)
        {
            var result = new byte[2];

            result[0] = (byte) (value >> 8);
            result[1] = (byte) value;

            return result;
        }

        public static short DecodeShort(byte[] value)
        {
            if ((value[0] | value[1]) < 0)
            {
                return -1;
            }

            return (short) ((value[0] << 8) + value[1]);
        }
    }
}
