using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Room.Chat
{
    /// <summary>
    ///     Warning, code is shared between
    ///     - ChatModifier
    ///     - ShoutModifier
    ///     - WhisperModifier
    /// </summary>
    [DefineOutgoingPacketModifier(new []
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.Shout)]
    public class ShoutModifier : PacketModifierBase
    {
        public ShoutModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int SenderVirtualId { get; set; }

        public string Message { get; set; }

        public int Emotion { get; set; }

        public int Colour { get; set; }

        public List<(string, string, bool)> Unknown0 { get; set; }

        public int Unknown1 { get; set; }

        public override void Parse()
        {
            SenderVirtualId = PacketOriginal.NextInt();
            Message = PacketOriginal.NextString();
            Emotion = PacketOriginal.NextInt();
            Colour = PacketOriginal.NextInt();
            Unknown0 = new List<(string, string, bool)>(PacketOriginal.NextInt());

            for (var i = 0; i < Unknown0.Capacity; i++)
            {
                var unknown0_1 = PacketOriginal.NextString();
                var unknown0_2 = PacketOriginal.NextString();
                var unknown0_3 = PacketOriginal.NextBool();

                Unknown0.Add((unknown0_1, unknown0_2, unknown0_3));
            }

            Unknown1 = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(SenderVirtualId);
            PacketModified.Append(Message);
            PacketModified.Append(Emotion);
            PacketModified.Append(Colour);
            PacketModified.Append(Unknown0.Count);

            foreach (var tuple in Unknown0)
            {
                PacketModified.Append(tuple.Item1);
                PacketModified.Append(tuple.Item2);
                PacketModified.Append(tuple.Item3);
            }

            PacketModified.Append(Unknown1);
        }
    }
}
