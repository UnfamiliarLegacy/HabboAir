using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Habbo.Data.HabboInventory;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Inventory.AvatarEffect
{
    [DefineOutgoingPacketModifier(new []
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.AvatarEffects)]
    public class AvatarEffectsModifier : PacketModifierBase
    {
        public AvatarEffectsModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public List<Habbo.Data.HabboInventory.AvatarEffect> Effects { get; set; }

        public override void Parse()
        {
            Effects = new List<Habbo.Data.HabboInventory.AvatarEffect>(PacketOriginal.NextInt());

            for (var i = 0; i < Effects.Capacity; i++)
            {
                Effects.Add(new Habbo.Data.HabboInventory.AvatarEffect
                {
                    SpriteId = PacketOriginal.NextInt(),
                    Type = (AvatarEffectType) PacketOriginal.NextInt(),
                    Duration = PacketOriginal.NextInt(),
                    Quantity = PacketOriginal.NextInt(),
                    TimeLeft = PacketOriginal.NextInt(),
                    Permanent = PacketOriginal.NextBool()
                });
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Effects.Count);

            foreach (var effect in Effects)
            {
                PacketModified.Append(effect.SpriteId);
                PacketModified.Append((int) effect.Type);
                PacketModified.Append(effect.Duration);
                PacketModified.Append(effect.Quantity);
                PacketModified.Append(effect.TimeLeft);
                PacketModified.Append(effect.Permanent);
            }
        }
    }
}
