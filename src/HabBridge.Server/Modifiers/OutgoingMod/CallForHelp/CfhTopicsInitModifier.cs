using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.CallForHelp;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.CallForHelp
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.CfhTopicsInit)]
    public class CfhTopicsInitModifier : PacketModifierBase
    {
        public CfhTopicsInitModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<CallForHelpCategoryData> Categories { get; set; }

        public override void Parse()
        {
            Categories = new List<CallForHelpCategoryData>(PacketOriginal.NextInt());

            for (var i = 0; i < Categories.Capacity; i++)
            {
                var category = new CallForHelpCategoryData
                {
                    Name = PacketOriginal.NextString(),
                    Topics = new List<CallForHelpTopicData>(PacketOriginal.NextInt())
                };

                for (var j = 0; j < category.Topics.Capacity; j++)
                {
                    category.Topics.Add(new CallForHelpTopicData
                    {
                        Name = PacketOriginal.NextString(),
                        Id = PacketOriginal.NextInt(),
                        Consequence = PacketOriginal.NextString()
                    });
                }

                Categories.Add(category);
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Categories.Count);

            foreach (var category in Categories)
            {
                PacketModified.Append(category.Name);
                PacketModified.Append(category.Topics.Count);

                foreach (var topic in category.Topics)
                {
                    PacketModified.Append(topic.Name);
                    PacketModified.Append(topic.Id);
                    PacketModified.Append(topic.Consequence);
                }
            }
        }
    }
}
