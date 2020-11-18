using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet;
using HabBridge.Server.Net.Packet.Interfaces;
using HabBridge.Server.Registers;

namespace HabBridge.Server.Modifiers
{
    public abstract class PacketModifierBase
    {
        public PacketModifierBase(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget)
        {
            Connection = connection;
            PacketOriginal = packetOriginal;
            ReleaseFrom = releaseFrom;
            ReleaseTarget = releaseTarget;
            PacketModified = CreatePacket(Registar.GetPacketIdModified(packetOriginal.Header, packetOriginal.Type, releaseFrom, releaseTarget));
        }

        public ClientConnection Connection { get; }

        public IPacket PacketOriginal { get; }
        public Release ReleaseFrom { get; }
        public Release ReleaseTarget { get; internal set; }
        public IPacket PacketModified { get; }

        /// <summary>
        ///     Set to true to ignore this packet.
        /// </summary>
        public bool Discard { get; protected set; }

        /// <summary>
        ///     Parses the packet.
        /// </summary>
        public abstract void Parse();

        /// <summary>
        ///     Creates the new, modified, packet.
        /// </summary>
        public abstract void Recreate();

        protected IPacket CreatePacket(short packetId)
        {
            return PacketOriginal.Type == PacketType.Incoming ? (IPacket)
                new PacketIncoming(ReleaseTarget, packetId) :
                new PacketOutgoing(ReleaseTarget, packetId);
        }

        protected IPacket CreatePacketIncoming(Incoming header)
        {
            return new PacketIncoming(ReleaseTarget, Registar.HeadersIncoming[ReleaseTarget][header]);   
        }

        protected IPacket CreatePacketOutgoing(Outgoing header)
        {
            return new PacketOutgoing(ReleaseTarget, Registar.HeadersOutgoing[ReleaseTarget][header]);   
        }
        
        /// <summary>
        ///     Allows the modifier to "add" additional packets to the target.
        /// </summary>
        /// <returns></returns>
        public virtual List<T> CreateExtraPackets<T>() where T : IPacket
        {
            return null;
        }
    }
}
