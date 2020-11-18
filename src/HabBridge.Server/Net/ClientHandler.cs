using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace HabBridge.Server.Net
{
    public class ClientHandler
    {
        private static readonly ILogger Logger = Log.ForContext<ClientHandler>();

        private CancellationTokenSource _cancellationToken;
        private Task _listenTask;
        private Socket _socket;

        public ClientHandler()
        {
            ClientConnections = new List<ClientConnection>();
        }

        public List<ClientConnection> ClientConnections { get; }

        private async Task ListenAsync(TaskCompletionSource<bool> listenStarted)
        {
            using (_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    var endpoint = new IPEndPoint(IPAddress.Any, Constants.ServerPort);
                    _socket.Bind(endpoint);
                    _socket.Listen(10);

                    Logger.Information($"Started listening on {endpoint} for game connections");

                    listenStarted.TrySetResult(true);
                }
                catch (Exception e)
                {
                    listenStarted.TrySetException(e);
                    return;
                }

                while (!_cancellationToken.IsCancellationRequested)
                {
                    Logger.Debug("Waiting for a connection..");

                    try
                    {
                        var clientConnection = new ClientConnection(this, await _socket.AcceptAsync());
                        new Thread(clientConnection.Start).Start();

                        ClientConnections.Add(clientConnection);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, "ClientHandler threw an exception");
                    }

                    Logger.Debug("Accepted a new connection");
                }

                Logger.Warning("Shutting down server socket");

                if (_socket.Connected)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
            }
        }

        public async Task StartAsync()
        {
            if (_listenTask != null)
            {
                throw new Exception("Handler is already started.");
            }

            var listenCompleted = new TaskCompletionSource<bool>();
            _cancellationToken = new CancellationTokenSource();
            _listenTask = ListenAsync(listenCompleted);

            await listenCompleted.Task;
        }

        public void Stop()
        {
            for (var i = ClientConnections.Count - 1; i >= 0; i--)
            {
                ClientConnections[i].Shutdown(null, false);
                ClientConnections[i].Dispose();
                ClientConnections.RemoveAt(i);
            }

            if (_socket != null && _socket.Connected) _socket.Shutdown(SocketShutdown.Both);

            _cancellationToken?.Cancel();
            _listenTask = null;
        }
    }
}
