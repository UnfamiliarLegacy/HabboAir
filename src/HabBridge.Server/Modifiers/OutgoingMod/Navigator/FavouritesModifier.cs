using System.Collections.Generic;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
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
    }, Outgoing.Favourites)]
    public class FavouritesModifier : PacketModifierBase
    {
        public FavouritesModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Limit { get; set; }

        public List<int> FavouriteIds { get; set; }

        public override void Parse()
        {
            Limit = PacketOriginal.NextInt();
            FavouriteIds = new List<int>(PacketOriginal.NextInt());

            for (var i = 0; i < FavouriteIds.Capacity; i++)
            {
                FavouriteIds.Add(PacketOriginal.NextInt());
            }
        }

        public override void Recreate()
        {
            PacketModified.Append(Limit);
            PacketModified.Append(FavouriteIds.Count);

            foreach (var id in FavouriteIds)
            {
                PacketModified.Append(id);
            }
        }
    }
}
