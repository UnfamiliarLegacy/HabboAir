using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Net.Encoders;
using HabBridge.Server.Net.Packet.Exceptions;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Net.Packet
{
    public abstract class PacketBase : IPacket
    {
        /// <summary>
        ///     Sets up the <see cref="PacketBase"/> instance for writing.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="release"></param>
        /// <param name="header">The packet id.</param>
        protected PacketBase(PacketType type, Release release, short header)
        {
            Type = type;
            Release = release;
            Header = header;
            Payload = new List<byte>();
            AllowRead = false;
            AllowWrite = true;

            Append(header);
        }

        /// <summary>
        ///     Sets up the <see cref="PacketBase"/> instance for reading.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="release"></param>
        /// <param name="packet"></param>
        protected PacketBase(PacketType type, Release release, IReadOnlyList<byte> packet)
        {
            Type = type;
            Release = release;
            Header = ByteEncoding.DecodeShort(new[] { packet[0], packet[1] });
            Pointer = 2; // Header is already read.
            Payload = packet.ToList();
            AllowRead = true;
            AllowWrite = false;
        }

        public PacketType Type { get; }

        public Release Release { get; }

        public short Header { get; }

        public int Length => Payload.Count;

        public int Pointer { get; set; }

        public int BytesLeft => Length - Pointer;

        public bool BytesAvailable => BytesLeft > 0;

        public List<byte> Payload { get; }

        public bool AllowRead { get; }

        public bool AllowWrite { get; }

        public bool NextBool()
        {
            if (!AllowRead)
            {
                throw new PacketUnreadableException("AllowRead is set to false, no permission to read.");
            }

            return Payload[Pointer++] == 0x01;
        }

        public short NextShort()
        {
            if (!AllowRead)
            {
                throw new PacketUnreadableException("AllowRead is set to false, no permission to read.");
            }

            return ByteEncoding.DecodeShort(new[]
            {
                Payload[Pointer++], Payload[Pointer++]
            });
        }

        public int NextInt()
        {
            if (!AllowRead)
            {
                throw new PacketUnreadableException("AllowRead is set to false, no permission to read.");
            }

            return ByteEncoding.DecodeInt(new[]
            {
                Payload[Pointer++], Payload[Pointer++],
                Payload[Pointer++], Payload[Pointer++]
            });
        }

        public string NextString()
        {
            if (!AllowRead)
            {
                throw new PacketUnreadableException("AllowRead is set to false, no permission to read.");
            }

            var length = NextShort();
            var bytes = new byte[length];

            Buffer.BlockCopy(Payload.ToArray(), Pointer, bytes, 0, length);
            Pointer += length;

            return Encoding.UTF8.GetString(bytes);
        }

        public byte[] NextBytes()
        {
            if (!AllowRead)
            {
                throw new PacketUnreadableException("AllowRead is set to false, no permission to read.");
            }

            var bytes = new byte[BytesLeft];

            Buffer.BlockCopy(Payload.ToArray(), Pointer, bytes, 0, BytesLeft);
            Pointer += BytesLeft;

            return bytes;
        }

        public IPacketWriter Append(bool value)
        {
            if (!AllowWrite)
            {
                throw new PacketUnwriteableException("AllowWrite is set to false, no permission to write.");
            }

            Payload.Add((byte) (value ? 1 : 0));

            return this;
        }

        public IPacketWriter Append(short value)
        {
            if (!AllowWrite)
            {
                throw new PacketUnwriteableException("AllowWrite is set to false, no permission to write.");
            }

            Payload.AddRange(ByteEncoding.EncodeShort(value));

            return this;
        }

        public IPacketWriter Append(int value)
        {
            if (!AllowWrite)
            {
                throw new PacketUnwriteableException("AllowWrite is set to false, no permission to write.");
            }

            Payload.AddRange(ByteEncoding.EncodeInt(value));

            return this;
        }

        public IPacketWriter Append(string value)
        {
            if (!AllowWrite)
            {
                throw new PacketUnwriteableException("AllowWrite is set to false, no permission to write.");
            }

            var valueBytes = HabboConstants.StringEncoding.GetBytes(value);

            Append((short) valueBytes.Length);
            Payload.AddRange(valueBytes);

            return this;
        }

        public IPacketWriter Append(IEnumerable<byte> bytes)
        {
            if (!AllowWrite)
            {
                throw new PacketUnwriteableException("AllowWrite is set to false, no permission to write.");
            }

            Payload.AddRange(bytes);

            return this;
        }

        public List<byte> GetPayload()
        {
            // Skip header
            return Payload.Skip(2).ToList();
        }

        public List<byte> GetPayloadLeft()
        {
            var amount = BytesLeft;
            if (amount == 0)
            {
                return new List<byte>(0);
            }

            return Payload.Skip(Pointer).ToList();
        }

        public List<byte> GetPacket()
        {
            var packet = new List<byte>(Payload);
            packet.InsertRange(0, ByteEncoding.EncodeInt(Payload.Count)); // +2 for the packet id.

            return packet;
        }
    }
}
