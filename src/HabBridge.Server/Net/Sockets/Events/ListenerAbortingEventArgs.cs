using System;

namespace HabBridge.Server.Net.Sockets.Events
{
    internal class ListenerAbortingEventArgs : EventArgs
    {
        public string Reason { get; protected internal set; }

        public Exception Exception { get; protected internal set; } = null;
    }
}
