using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.Navigator;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Navigator
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.UserFlatCats)]
    public class UserFlatCatsModifier : PacketModifierBase
    {
        public UserFlatCatsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<FlatCategory> Categories { get; set; }

        public override void Parse()
        {
            Categories = new List<FlatCategory>(PacketOriginal.NextInt());

            for (var i = 0; i < Categories.Capacity; i++)
            {
                Categories.Add(new FlatCategory
                {
                    Id = PacketOriginal.NextInt(),
                    NodeName = PacketOriginal.NextString(),
                    Visible = PacketOriginal.NextBool(),
                    Automatic = PacketOriginal.NextBool(),
                    Unknown0 = PacketOriginal.NextString(),
                    Unknown1 = PacketOriginal.NextString(),
                    Unknown2 = PacketOriginal.NextBool()
                });
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Categories.Count);

            foreach (var category in Categories)
            {
                PacketModified.Append(category.Id);
                PacketModified.Append(category.NodeName);
                PacketModified.Append(category.Visible);
                PacketModified.Append(category.Automatic);
                PacketModified.Append(category.Unknown0);
                PacketModified.Append(category.Unknown1);
                PacketModified.Append(category.Unknown2);
            }
        }
    }
}
