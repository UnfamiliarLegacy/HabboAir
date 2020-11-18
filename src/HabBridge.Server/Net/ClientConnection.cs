using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using HabBridge.Hotels;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Session;
using HabBridge.Server.Habbo.Utils;
using HabBridge.Server.Modifiers.IncomingMod.Handshake;
using HabBridge.Server.Modifiers.OutgoingMod.Navigator;
using HabBridge.Server.Net.Packet;
using HabBridge.Server.Net.Packet.Events;
using HabBridge.Server.Net.Sockets;
using HabBridge.Server.Net.Sockets.Events;
using HabBridge.Server.Utils;
using HabCrypto;
using Serilog;

namespace HabBridge.Server.Net
{
    public class ClientConnection
    {
        private static readonly ILogger Logger = Log.ForContext<ClientConnection>();
        
        private readonly PacketModifier _packetModifier;

        private bool _isStarted;
        private bool _shuttingDown;

        public ClientConnection(ClientHandler clientHandler, Socket clientSocket)
        {
            Handler = clientHandler;
            Session = new HabboSession();

            Client = new HabboConnection(
                new SocketListener(clientSocket, 1024),
                new PacketProcessor(this, PacketType.Incoming, Constants.ServerRelease),
                new HabboEncryption(Constants.AirServerExponent, Constants.AirServerModulus, Constants.AirServerPrivateExponent)
            );

            Client.Listener.Listening += ClientListenerOnListening;
            Client.Listener.Aborting += ClientListenerOnAborting;
            Client.Listener.ReceivedBytes += ClientListenerOnReceivedBytes;

            Client.PacketProcessor.PacketReceived += ClientPacketProcessorOnPacketReceived;

            _packetModifier = new PacketModifier(this, Constants.ServerRelease, Constants.ServerRelease, false);
            _packetModifier.ParsedIncomingPacket += PacketModifierOnParsedIncomingPacket;
            _packetModifier.ParsedOutgoingPacket += PacketModifierOnParsedOutgoingPacket;
        }

        public ClientHandler Handler { get; }

        public HabboSession Session { get; }

        /// <summary>
        ///     Connection between us and the habbo mobile client.
        /// </summary>
        internal HabboConnection Client { get; }

        /// <summary>
        ///     Connection between us and the habbo retro emulator.
        /// </summary>
        internal HabboConnection Server { get; private set; }

        public Incoming LastIncomingPacket { get; set; }

        public Outgoing LastOutgoingPacket { get; set; }

        public bool ReleaseVersionReceived { get; private set; }

        public HotelConfig Config { get; private set; }

        public void Start()
        {
            if (_isStarted)
            {
                return;
            }

            _isStarted = true;
            
            Task.Run(Client.Listener.StartListeningAsync).ConfigureAwait(false);
        }

        private void StartServerConnection(HotelConfig hotelConfig)
        {
            if (Config != null)
            {
                throw new ApplicationException("ServerConnection was already configured.");
            }

            Config = hotelConfig;

            // Setup packet modifier.
            _packetModifier.ServerRelease = Config.Release;

            // Setup server stuff.
            Server = new HabboConnection(
                new SocketListener(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), 1024),
                new PacketProcessor(this, PacketType.Outgoing, Config.Release),
                new HabboEncryption(Config.Exponent, Config.Modulus)
            );

            Server.Listener.Listening += ServerListenerOnListening;
            Server.Listener.Aborting += ServerListenerOnAborting;
            Server.Listener.ReceivedBytes += ServerListenerOnReceivedBytes;

            Server.PacketProcessor.PacketReceived += ServerPacketProcessorOnPacketReceived;

            // Open connection.
            Server.Listener.Socket.Connect(Config.ServerIp, Config.ServerPort); // TODO: Handle exception?

            if (!Server.Listener.Socket.Connected)
            {
                Shutdown("Unable to connect to target server.");
            }
            
            // Start listening.
            Task.Run(Server.Listener.StartListeningAsync).ConfigureAwait(false);
        }

        public void Shutdown(string reason = null, bool remove = true)
        {
            _shuttingDown = true;

            Logger.Warning(reason == null
                ? "Stopping game connection"
                : $"Stopping game connection with the reason: \"{reason}\"");

            // TODO: Figure out why there is a NullReferenceException here.
            // Triggered by closing the game client and then pressing CTRL+C in the console window.

            if (Client.Listener.Socket.Connected)
            {
                Client.Listener.Socket.Shutdown(SocketShutdown.Both);
            }

            // Server can be null when the connection was only for XML policy.
            if (Server?.Listener != null && Server.Listener.Socket.Connected)
            {
                Server.Listener.Socket.Shutdown(SocketShutdown.Both);
            }

            if (remove)
            {
                Handler.ClientConnections.Remove(this);

                Dispose();
            }
        }

        // Client
        private void ClientListenerOnListening(object sender, EventArgs e)
        {
            Logger.Verbose("ClientListenerOnListening");
        }

        // Client
        private void ClientListenerOnReceivedBytes(object sender, ListenerReceivedBytesEventArgs e)
        {
            Logger.Verbose($"ClientListenerOnReceivedBytes({e.Bytes.Length})");

            var bytes = e.Bytes;

            // Policy
            if (bytes.Length == 23 && bytes[0] == 0x3c)
            {
                SendToClientAsync(CrossDomainPolicy.GetPolicyBytes()).GetAwaiter().GetResult();
            }
            else
            {
                Client.Encryption.ServerRC4?.Parse(bytes);
                Client.PacketProcessor.ParseBytes(bytes);
            }
        }

        // Client
        private void ClientListenerOnAborting(object sender, ListenerAbortingEventArgs e)
        {
            if (_shuttingDown)
            {
                // Shutdown was expected.
                return;
            }

            // Shutdown was unexpected.
            if (e.Exception != null)
            {
                Logger.Error(e.Exception, $"ClientListener aborted with the reason: \"{e.Reason}\".");
            }
            else
            {
                Logger.Error($"ClientListener aborted with the reason: \"{e.Reason}\".");
            }

            Shutdown("ClientListener closed the connection");
        }

        // Client
        private void ClientPacketProcessorOnPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var result = _packetModifier.ModifyPacket<PacketIncoming>(e.Packet, out var manualDiscard);
            if (result != null)
            {
                // Log.
                Logger.Debug($"Client packet received [{e.Packet.Header,-4}] {result.PacketModified.HeaderType}");

                if (result.ExtraPackets != null)
                {
                    foreach (var packet in result.ExtraPackets)
                    {
                        Logger.Debug($"Client packet extra for {result.PacketModified.HeaderType} > {packet.HeaderType}.");
                    }
                }
                
                // Send.
                Task.Run(() => SendToServerAsync(result.PacketModified));
                
                if (result.ExtraPackets != null)
                {
                    foreach (var packet in result.ExtraPackets)
                    {
                        Task.Run(() => SendToServerAsync(packet));
                    }
                }
            }
            else if (manualDiscard)
            {
                Logger.Debug("Client packet {header} has been discarded manually.", e.Packet.Header);
            }
            else
            {
                Logger.Warning("Client packet {header} has been discarded, release {release}. Hex:\n" + 
                               HexUtil.HexDump(e.Packet.GetPacket().ToArray()), e.Packet.Header, e.Packet.Release);
            }
        }

        // Client -> Us -> Server (Incoming)
        private void PacketModifierOnParsedIncomingPacket(object sender, PacketParsedEventArgs e)
        {
            switch (e.PacketModifier)
            {
                case ReleaseVersionModifier modifier:
                {
                    if (ReleaseVersionReceived)
                    {
                        throw new ApplicationException("HotelType was already sent by client.");
                    }

                    ReleaseVersionReceived = true;

                    if (Enum.TryParse<HotelType>(modifier.HotelType, true, out var hotelType))
                    {
                        var hotel = HotelManager.GetHotel(hotelType);
                        if (hotel != null)
                        {
                            // Open connection with the target server.
                            StartServerConnection(hotel);

                            // Apply the target release to this modifier.
                            // All future modifiers will automatically get this modifier as well.
                            modifier.ReleaseTarget = hotel.Release;
                            break;
                        }
                    }

                    throw new ApplicationException("HotelType was not sent properly by client.");
                }

                case SwfDataModifier modifier:
                {
                    modifier.FlashClientUrl = Config.FlashClientUrl;
                    modifier.ExternalVariables = Config.ExternalVariables;
                    break;
                }

                // Client sends his public key to the server.
                case GenerateSecretKeyModifier modifier:
                {
                    var sharedKeyClient = Client.Encryption.Diffie.GetSharedKey(modifier.PublicKey);

                    Client.Encryption.ServerRC4 = new HabboRC4(sharedKeyClient);
                    Client.Encryption.ClientRC4 = new HabboRC4(sharedKeyClient);

                    modifier.PublicKey = Server.Encryption.Diffie.GetPublicKey();
                    break;
                }
            }
        }

        // Server
        private void ServerListenerOnListening(object sender, EventArgs e)
        {
            Logger.Verbose("ServerListenerOnListening");
        }

        // Server
        private void ServerListenerOnReceivedBytes(object sender, ListenerReceivedBytesEventArgs e)
        {
            Logger.Verbose($"ServerListenerOnReceivedBytes({e.Bytes.Length})");

            var bytes = e.Bytes;

            Server.Encryption.ServerRC4?.Parse(bytes);
            Server.PacketProcessor.ParseBytes(bytes);
        }

        // Server
        private void ServerListenerOnAborting(object sender, ListenerAbortingEventArgs e)
        {
            if (_shuttingDown)
            {
                // Shutdown was expected.
                return;
            }

            // Shutdown was unexpected.
            if (e.Exception != null)
            {
                Logger.Error(e.Exception, $"ServerListener aborted with the reason: \"{e.Reason}\".");
            }
            else
            {
                Logger.Error($"ServerListener aborted with the reason: \"{e.Reason}\".");
            }

            Shutdown("ServerListener closed the connection");
        }

        // Server
        private void ServerPacketProcessorOnPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var result = _packetModifier.ModifyPacket<PacketOutgoing>(e.Packet, out var manualDiscard);
            if (result != null)
            {
                // Log.
                Logger.Debug($"Server packet received [{e.Packet.Header,-4}] {result.PacketModified.HeaderType}");

                if (result.ExtraPackets != null)
                {
                    foreach (var packet in result.ExtraPackets)
                    {
                        Logger.Debug($"Server packet extra for {result.PacketModified.HeaderType} > {packet.HeaderType}.");
                    } 
                }
                
                // Send.
                Task.Run(() => SendToClientAsync(result.PacketModified));
                
                if (result.ExtraPackets != null)
                {
                    foreach (var packet in result.ExtraPackets)
                    {
                        Task.Run(() => SendToClientAsync(packet));
                    }
                }
            }
            else if (manualDiscard)
            {
                Logger.Debug("Server packet {header} has been discarded manually.", e.Packet.Header);
            }
            else
            {
                Logger.Warning("Server packet {header} has been discarded, release {release}. Hex:\n" + 
                               HexUtil.HexDump(e.Packet.GetPacket().ToArray()), e.Packet.Header, e.Packet.Release);
            }
        }

        // Server -> Us -> Client (Outgoing)
        private void PacketModifierOnParsedOutgoingPacket(object sender, PacketParsedEventArgs e)
        {
            switch (e.PacketModifier)
            {
                // Server sends prime (p) and generator (g) to the client.
                case Modifiers.OutgoingMod.Handshake.InitCryptoModifier modifier:
                {
                    Server.Encryption.Diffie.DoHandshake(modifier.SignedPrime, modifier.SignedGenerator);

                    modifier.SignedPrime = Client.Encryption.Diffie.GetSignedPrime();
                    modifier.SignedGenerator = Client.Encryption.Diffie.GetSignedGenerator();
                    break;
                }

                // Server sends his public key to the client.
                case Modifiers.OutgoingMod.Handshake.SecretKeyModifier modifier:
                {
                    var sharedKeyServer = Server.Encryption.Diffie.GetSharedKey(modifier.PublicKey);

                    Server.Encryption.ServerRC4 = new HabboRC4(sharedKeyServer);
                    Server.Encryption.ClientRC4 = new HabboRC4(sharedKeyServer);

                    if (!modifier.EnableClientEncryption)
                    {
                        Client.Encryption.ClientRC4 = null; // We will not send encrypted packets to the habbo client.
                        Server.Encryption.ServerRC4 = null; // The habbo server won't send us encrypted packets.
                    }

                    modifier.PublicKey = Client.Encryption.Diffie.GetPublicKey();
                    break;
                }

                case UserFlatCatsModifier modifier:
                {
                    Session.Navigator.UserFlatCatMapping.Clear();

                    foreach (var category in modifier.Categories)
                    {
                        Session.Navigator.UserFlatCatMapping.Add(category.Id, category.NodeName);
                    }
                    break;
                }
            }
        }

        private async Task SendToServerAsync(PacketIncoming packet)
        {
            // Get packet bytes.
            var bytes = packet.GetPacket().ToArray();

            Server.Encryption.ClientRC4?.Parse(bytes);

            // Do tracking.
            LastIncomingPacket = packet.HeaderType;

            // Send.
            await Server.Listener.Socket.SendAsync(bytes, SocketFlags.None);
        }

        private async Task SendToClientAsync(PacketOutgoing packet)
        {
            // Get packet bytes.
            var bytes = packet.GetPacket().ToArray();

            Client.Encryption.ClientRC4?.Parse(bytes);

            // Do tracking.
            LastOutgoingPacket = packet.HeaderType;

            // Send.
            await SendToClientAsync(bytes);
        }

        private async Task SendToClientAsync(byte[] bytes)
        {
            await Client.Listener.Socket.SendAsync(bytes, SocketFlags.None);
        }

        public void Dispose()
        {
            Client?.Dispose();
            Server?.Dispose();
        }
    }
}
