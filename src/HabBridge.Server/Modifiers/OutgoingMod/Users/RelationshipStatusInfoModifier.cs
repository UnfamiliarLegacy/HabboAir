using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Users;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Users
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.RelationshipStatusInfo)]
    public class RelationshipStatusInfoModifier : PacketModifierBase
    {
        public RelationshipStatusInfoModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int UserId { get; set; }

        public List<RelationshipStatusInfo> Relationships { get; set; }

        public override void Parse()
        {
            UserId = PacketOriginal.NextInt();
            Relationships = new List<RelationshipStatusInfo>(PacketOriginal.NextInt());

            for (var i = 0; i < Relationships.Capacity; i++)
            {
                Relationships.Add(new RelationshipStatusInfo
                {
                    Type = (RelationshipStatusType) PacketOriginal.NextInt(),
                    Amount = PacketOriginal.NextInt(),
                    TargetUserId = PacketOriginal.NextInt(),
                    TargetUsername = PacketOriginal.NextString(),
                    TargetFigure = PacketOriginal.NextString()
                });
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(UserId);
            PacketModified.Append(Relationships.Count);

            foreach (var relationship in Relationships)
            {
                PacketModified.Append((int) relationship.Type);
                PacketModified.Append(relationship.Amount);
                PacketModified.Append(relationship.TargetUserId);
                PacketModified.Append(relationship.TargetUsername);
                PacketModified.Append(relationship.TargetFigure);
            }
        }
    }
}
