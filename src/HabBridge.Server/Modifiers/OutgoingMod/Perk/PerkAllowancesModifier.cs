using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.HabboBasicInfo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Perk
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.PerkAllowances)]
    public class PerkAllowancesModifier : PacketModifierBase
    {
        public PerkAllowancesModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<UserPerk> Perks { get; set; }

        public override void Parse()
        {
            Perks = new List<UserPerk>(PacketOriginal.NextInt());

            for (var i = 0; i < Perks.Capacity; i++)
            {
                Perks.Add(new UserPerk
                {
                    Code = PacketOriginal.NextString(),
                    ErrorMessage = PacketOriginal.NextString(),
                    IsAllowed = PacketOriginal.NextBool()
                });
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Perks.Count);

            foreach (var perk in Perks)
            {
                PacketModified.Append(perk.Code);
                PacketModified.Append(perk.ErrorMessage);
                PacketModified.Append(perk.IsAllowed);
            }
        }
    }

    
}
