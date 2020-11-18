using System;
using System.Threading;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Net.Encoders;
using HabBridge.Server.Net.Packet.Events;
using HabBridge.Server.Net.Packet.Interfaces;
using Serilog;

namespace HabBridge.Server.Net.Packet
{
    /// <inheritdoc cref="IPacketProcessor"/>
    internal class PacketProcessor : IPacketProcessor
    {
        private static readonly ILogger Logger = Log.ForContext<PacketProcessor>();

        public PacketProcessor(ClientConnection connection, PacketType type, Release release)
        {
            Connection = connection;
            Type = type;
            TypeClass = Type == PacketType.Incoming ? typeof(PacketIncoming) : typeof(PacketOutgoing);
            Release = release;
            ByteBuffer = new byte[0];
            Lock = new Semaphore(1, 1);
        }

        private ClientConnection Connection { get; }

        private PacketType Type { get; }

        private Type TypeClass { get; }

        private Release Release { get; }

        private byte[] ByteBuffer { get; set; }

        private Semaphore Lock { get; }

        public void ParseBytes(byte[] receivedBytes)
        {
            Lock.WaitOne();

            try
            {
                var bytes = new byte[ByteBuffer.Length + receivedBytes.Length];
                Buffer.BlockCopy(ByteBuffer, 0, bytes, 0, ByteBuffer.Length);
                Buffer.BlockCopy(receivedBytes, 0, bytes, ByteBuffer.Length, receivedBytes.Length);

                var pointer = 0;
                var keepSearching = true;
                while (keepSearching)
                {
                    // Check if atleast the length is available
                    if (bytes.Length > pointer)
                    {
                        var length = ByteEncoding.DecodeInt(new[]
                        {
                            bytes[pointer], bytes[pointer + 1],
                            bytes[pointer + 2], bytes[pointer + 3]
                        });

                        if (bytes.Length >= pointer + 4 + length)
                        {
                            var packet = new byte[length];
                            Buffer.BlockCopy(bytes, pointer + 4, packet, 0, length);
                            
                            OnPacketReceived(new PacketReceivedEventArgs
                            {
                                Packet = (PacketBase) Activator.CreateInstance(TypeClass, Release, packet)
                            });

                            pointer = pointer + 4 + length;
                        }
                        else
                        {
                            keepSearching = false;
                        }
                    }
                    else
                    {
                        keepSearching = false;
                    }
                }

                var leftovers = bytes.Length - pointer;
                ByteBuffer = new byte[leftovers];

                if (leftovers != 0)
                {
                    Buffer.BlockCopy(bytes, pointer, ByteBuffer, 0, bytes.Length - pointer);
                }
            }
            catch (Exception e)
            {
                var message = $"Something went wrong in the packet processor of the packet type {Type} {Release}.";
                
                Logger.Error(e, message);
                Connection.Shutdown(message);
            }
            finally
            {
                Lock.Release();
            }
        }

        public void OnPacketReceived(PacketReceivedEventArgs eventArgs)
        {
            PacketReceived?.Invoke(this, eventArgs);
        }

        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
    }
}
