using System;
using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Modifiers;
using HabBridge.Server.Net.Packet.Events;
using HabBridge.Server.Net.Packet.Exceptions;
using HabBridge.Server.Net.Packet.Interfaces;
using HabBridge.Server.Registers;
using Serilog;
using Serilog.Events;

namespace HabBridge.Server.Net.Packet
{
    public class PacketModifier
    {
        private static readonly ILogger Logger = Log.ForContext<PacketModifier>();

        private readonly bool _forceModification;

        /// <summary>
        ///     Initializes the <see cref="PacketModifier"/>.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="clientRelease">Release of the habbo client.</param>
        /// <param name="serverRelease">Release of the habbo server.</param>
        /// <param name="forceModification">Whether a modified packet is required.</param>
        public PacketModifier(ClientConnection connection, Release clientRelease, Release serverRelease, bool forceModification)
        {
            Connection = connection;
            ClientRelease = clientRelease;
            ServerRelease = serverRelease;

            _forceModification = forceModification;
        }

        public ClientConnection Connection { get; }

        /// <summary>
        ///     Release of the habbo client.
        /// </summary>
        public Release ClientRelease { get; }

        /// <summary>
        ///     Release of the habbo server.
        /// </summary>
        public Release ServerRelease { get; set; }

        public PacketModifierResult<T> ModifyPacket<T>(IPacket packet, out bool manualDiscard) where T : IPacket
        {
            manualDiscard = false;

            var clientModifierType = packet.Type == PacketType.Incoming
                ? Registar.ModifiersIncoming[ClientRelease].GetValueOrDefault(((PacketIncoming) packet).HeaderType)
                : Registar.ModifiersOutgoing[ClientRelease].GetValueOrDefault(((PacketOutgoing) packet).HeaderType);

            var serverModifierType = packet.Type == PacketType.Incoming
                ? Registar.ModifiersIncoming[ServerRelease].GetValueOrDefault(((PacketIncoming) packet).HeaderType)
                : Registar.ModifiersOutgoing[ServerRelease].GetValueOrDefault(((PacketOutgoing) packet).HeaderType);

            if (clientModifierType != null &&
                serverModifierType != null && 
                clientModifierType == serverModifierType)
            {
                // Modifier supports both releases.
                var modifier = (PacketModifierBase) Activator.CreateInstance(clientModifierType,
                    Connection,
                    packet,
                    packet.Release,
                    packet.Type == PacketType.Incoming ? ServerRelease : ClientRelease
                );

                modifier.Parse();

                // Notify listener about the parse.
                if (packet.Type == PacketType.Incoming)
                {
                    OnParsedIncomingPacket(new PacketParsedEventArgs
                    {
                        PacketModifier = modifier
                    });
                }
                else
                {
                    OnParsedOutgoingPacket(new PacketParsedEventArgs
                    {
                        PacketModifier = modifier
                    });
                }

                modifier.Recreate();

                if (modifier.Discard)
                {
                    manualDiscard = true;
                    return default;
                }

                if (modifier.PacketOriginal.BytesLeft > 0)
                {
                    Logger.Warning($"PacketModifier: Not all bytes were read for {packet.Type.ToString().ToLower()} packet id {packet.Header}, {modifier.PacketOriginal.BytesLeft} byte(s) left.");
                }
                
                if (Logger.IsEnabled(LogEventLevel.Verbose))
                {
                    var packetType = packet.Type == PacketType.Incoming
                        ? ((PacketIncoming)packet).HeaderType.ToString()
                        : ((PacketOutgoing)packet).HeaderType.ToString();

                    Logger.Verbose($"- Packet type  : {packetType}");
                    Logger.Verbose($"- Packet id    : {modifier.PacketOriginal.Header,-6} => {modifier.PacketModified.Header,-6}");
                    Logger.Verbose($"- Packet length: {modifier.PacketOriginal.Length,-6} => {modifier.PacketModified.Length,-6}");
                }

                return new PacketModifierResult<T>((T) modifier.PacketModified, modifier.CreateExtraPackets<T>());
            }

            if (_forceModification || (packet.Type == PacketType.Incoming && ((PacketIncoming)packet).Header == 4000))
            {
                throw new UnidentifiedPacketModifierException($"Unable to find a compatible packet modifier for R({packet.Release}) T({packet.Type}) H({packet.Header}).");
            }

            // Should be no compatibility issues.
            if (ClientRelease == ServerRelease)
            {
                return new PacketModifierResult<T>((T) packet);
            }

            return default;
        }

        private void OnParsedIncomingPacket(PacketParsedEventArgs e)
        {
            ParsedIncomingPacket?.Invoke(this, e);
        }

        private void OnParsedOutgoingPacket(PacketParsedEventArgs e)
        {
            ParsedOutgoingPacket?.Invoke(this, e);
        }

        public event EventHandler<PacketParsedEventArgs> ParsedIncomingPacket;

        public event EventHandler<PacketParsedEventArgs> ParsedOutgoingPacket;
    }
}
