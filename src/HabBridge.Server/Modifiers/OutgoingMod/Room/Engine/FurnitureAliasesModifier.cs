using System;
using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Room.Engine
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.FurnitureAliases)]
    public class FurnitureAliasesModifier : PacketModifierBase
    {
        public FurnitureAliasesModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<Tuple<string, string>> Aliases { get; set; }

        public override void Parse()
        {
            Aliases = new List<Tuple<string, string>>(PacketOriginal.NextInt());

            for (var i = 0; i < Aliases.Capacity; i++)
            {
                var one = PacketOriginal.NextString();
                var two = PacketOriginal.NextString();

                Aliases.Add(new Tuple<string, string>(one, two));
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Aliases.Count);

            foreach (var alias in Aliases)
            {
                PacketModified.Append(alias.Item1);
                PacketModified.Append(alias.Item2);
            }
        }
    }
}