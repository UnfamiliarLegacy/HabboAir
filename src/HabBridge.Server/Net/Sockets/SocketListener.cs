using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using HabBridge.Server.Net.Sockets.Events;
using HabBridge.Server.Net.Sockets.Extensions;

namespace HabBridge.Server.Net.Sockets
{
    internal class SocketListener : IDisposable
    {
        private bool _isListening;

        public SocketListener(Socket socket, int bufferSize)
        {
            Socket = socket;
            BufferSize = bufferSize;
        }

        public Socket Socket { get; }

        public int BufferSize { get; }

        public async Task StartListeningAsync()
        {
            if (_isListening) return;
            _isListening = true;

            OnListening();

            var buffer = new byte[BufferSize];

            int bytesRead;
            while ((bytesRead = await Socket.ReceiveAsync(buffer)) > 0)
            {
                var receivedBytes = new byte[bytesRead];
                Buffer.BlockCopy(buffer, 0, receivedBytes, 0, bytesRead);

                OnReceivedBytes(new ListenerReceivedBytesEventArgs
                {
                    Bytes = receivedBytes
                });
            }

            // Read gave 0 bytes.
            OnAborting(new ListenerAbortingEventArgs
            {
                Reason = "Game closed connection"
            });
        }

        private void OnListening()
        {
            Listening?.Invoke(this, EventArgs.Empty);
        }

        private void OnAborting(ListenerAbortingEventArgs e)
        {
            Aborting?.Invoke(this, e);
        }

        private void OnReceivedBytes(ListenerReceivedBytesEventArgs e)
        {
            ReceivedBytes?.Invoke(this, e);
        }

        public event EventHandler<EventArgs> Listening;

        public event EventHandler<ListenerAbortingEventArgs> Aborting;

        public event EventHandler<ListenerReceivedBytesEventArgs> ReceivedBytes;

        public void Dispose()
        {
            Socket?.Dispose();
        }
    }
}
