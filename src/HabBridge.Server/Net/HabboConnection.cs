using System;
using HabBridge.Server.Net.Packet;
using HabBridge.Server.Net.Sockets;
using HabCrypto;

namespace HabBridge.Server.Net
{
    internal class HabboConnection : IDisposable
    {
        public HabboConnection(SocketListener listener, PacketProcessor packetProcessor, HabboEncryption encryption)
        {
            Listener = listener;
            PacketProcessor = packetProcessor;
            Encryption = encryption;
        }
        
        public SocketListener Listener { get; }

        public PacketProcessor PacketProcessor { get; }

        public HabboEncryption Encryption { get; }

        public void Dispose()
        {
            Listener?.Dispose();
        }
    }
}
