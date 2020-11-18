using System;
using HabBridge.Server.Net.Packet.Events;

namespace HabBridge.Server.Net.Packet.Interfaces
{
    /// <summary>
    ///     Processes packet received from the Habbo client.
    /// </summary>
    public interface IPacketProcessor
    {
        /// <summary>
        ///     Processes the received bytes from the Habbo client.
        /// </summary>
        /// <param name="receivedBytes"></param>
        void ParseBytes(byte[] receivedBytes);

        /// <summary>
        ///     Should invoke <see cref="PacketReceived"/>.
        /// </summary>
        void OnPacketReceived(PacketReceivedEventArgs eventArgs);

        event EventHandler<PacketReceivedEventArgs> PacketReceived;
    }
}
