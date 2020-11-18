using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Notifications
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.MOTDNotification)]
    public class MOTDNotificationModifier : PacketModifierBase
    {
        public MOTDNotificationModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<string> Messages { get; set; }

        public override void Parse()
        {
            Messages = new List<string>(PacketOriginal.NextInt());

            for (var i = 0; i < Messages.Capacity; i++)
            {
                Messages.Add(PacketOriginal.NextString());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Messages.Count);

            foreach (var message in Messages)
            {
                PacketModified.Append(message);
            }
        }
    }
}
