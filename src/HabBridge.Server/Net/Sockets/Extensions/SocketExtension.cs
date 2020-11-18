using System.Net.Sockets;
using System.Threading.Tasks;

namespace HabBridge.Server.Net.Sockets.Extensions
{
    internal static class SocketExtension
    {
        public static async Task SendAsync(this Socket socket, byte[] data)
        {
            var completed = new TaskCompletionSource<bool>();

            using (var eventArgs = new SocketAsyncEventArgs())
            {
                eventArgs.SetBuffer(data, 0, data.Length);
                eventArgs.Completed += (sender, args) =>
                {
                    if (args.SocketError == SocketError.Success)
                    {
                        completed.TrySetResult(true);
                    }
                    else
                    {
                        throw new SocketException((int)args.SocketError);
                    }
                };

                if (!socket.SendAsync(eventArgs))
                {
                    return; // Completed synchronously.
                }
            }

            await completed.Task;
        }

        public static async Task<int> ReceiveAsync(this Socket socket, byte[] buffer)
        {
            return await ReceiveAsync(socket, buffer, 0, buffer.Length);
        }

        public static async Task<int> ReceiveAsync(this Socket socket, byte[] buffer, int offset, int count)
        {
            var completed = new TaskCompletionSource<int>();

            using (var eventArgs = new SocketAsyncEventArgs())
            {
                eventArgs.SetBuffer(buffer, offset, count);
                eventArgs.Completed += (sender, args) =>
                {
                    completed.TrySetResult(args.SocketError == SocketError.Success ? args.BytesTransferred : 0);
                };

                if (!socket.ReceiveAsync(eventArgs))
                {
                    return eventArgs.BytesTransferred; // Completed synchronously.
                }
            }

            return await completed.Task;
        }
    }
}
