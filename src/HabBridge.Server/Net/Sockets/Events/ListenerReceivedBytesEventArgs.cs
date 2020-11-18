using System;

namespace HabBridge.Server.Net.Sockets.Events
{
    internal class ListenerReceivedBytesEventArgs : EventArgs
    {
        public byte[] Bytes { get; protected internal set; }
    }
}
