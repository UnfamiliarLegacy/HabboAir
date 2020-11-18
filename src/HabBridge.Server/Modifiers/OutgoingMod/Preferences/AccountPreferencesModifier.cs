using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;
using HabBridge.Server.Modifiers.Attributes;
using HabBridge.Server.Net;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Modifiers.OutgoingMod.Preferences
{
    [DefineOutgoingPacketModifier(new[]
    {
        Release.PRODUCTION_201607262204_86871104,
        Release.PRODUCTION_201701242205_837386174,
        Release.AIR63_201911271159_623255659
    }, Outgoing.AccountPreferences)]
    public class AccountPreferencesModifier : PacketModifierBase
    {
        public AccountPreferencesModifier(ClientConnection connection, IPacket packetOriginal, Release releaseFrom, Release releaseTarget) : base(connection, packetOriginal, releaseFrom, releaseTarget)
        {
        }

        public int Volume1 { get; set; }

        public int Volume2 { get; set; }

        public int Volume3 { get; set; }

        public bool ChatPreference { get; set; } // Old / New chat ?

        public bool InvitesStatus { get; set; }

        public bool FocusPreference { get; set; }

        public int FriendBarState { get; set; }

        public int Unknown0 { get; set; }

        public override void Parse()
        {
            Volume1 = PacketOriginal.NextInt();
            Volume2 = PacketOriginal.NextInt();
            Volume3 = PacketOriginal.NextInt();
            ChatPreference = PacketOriginal.NextBool();
            InvitesStatus = PacketOriginal.NextBool();
            FocusPreference = PacketOriginal.NextBool();
            FriendBarState = PacketOriginal.NextInt();
            Unknown0 = PacketOriginal.NextInt();
        }

        public override void Recreate()
        {
            PacketModified.Append(Volume1);
            PacketModified.Append(Volume2);
            PacketModified.Append(Volume3);
            PacketModified.Append(ChatPreference);
            PacketModified.Append(InvitesStatus);
            PacketModified.Append(FocusPreference);
            PacketModified.Append(FriendBarState);
            PacketModified.Append(Unknown0);
        }
    }
}
