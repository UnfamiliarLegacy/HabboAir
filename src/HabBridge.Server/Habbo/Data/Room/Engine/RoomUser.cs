using System;
using System.Collections.Generic;
using System.Globalization;
using HabBridge.Server.Net.Packet.Interfaces;

namespace HabBridge.Server.Habbo.Data.Room.Engine
{
    public class RoomUser
    {
        public RoomUser(IPacketReader packet)
        {
            UserId = packet.NextInt();
            DisplayName = packet.NextString();
            Motto = packet.NextString();
            Figure = packet.NextString();
            VirtualId = packet.NextInt();
            X = packet.NextInt();
            Y = packet.NextInt();
            Z = double.Parse(packet.NextString(), CultureInfo.InvariantCulture);
            Unknown0 = packet.NextInt();
            Unknown1 = (RoomUserType) packet.NextInt();

            switch (Unknown1)
            {
                case RoomUserType.User:
                    UserGender = packet.NextString();
                    UserUnknown0 = packet.NextInt();
                    UserUnknown1 = packet.NextInt();
                    UserUnknown2 = packet.NextString();
                    UserUnknown3 = packet.NextString();
                    UserUnknown4 = packet.NextInt();
                    UserUnknown5 = packet.NextBool();
                    break;
                case RoomUserType.Pet:
                    PetUnknown0 = packet.NextInt();
                    PetUnknown1 = packet.NextInt();
                    PetUnknown2 = packet.NextString();
                    PetUnknown3 = packet.NextInt();
                    PetUnknown4 = packet.NextBool();
                    PetUnknown5 = packet.NextBool();
                    PetUnknown6 = packet.NextBool();
                    PetUnknown7 = packet.NextBool();
                    PetUnknown8 = packet.NextBool();
                    PetUnknown9 = packet.NextBool();
                    PetUnknown10 = packet.NextInt();
                    PetUnknown11 = packet.NextString();
                    break;
                case RoomUserType.Unknown1:
                    // Nothing.
                    break;
                case RoomUserType.Bot:
                    BotUnknown0 = packet.NextString();
                    BotUnknown1 = packet.NextInt();
                    BotUnknown2 = packet.NextString();
                    BotUnknown3 = new List<short>(packet.NextInt());

                    for (var i = 0; i < BotUnknown3.Capacity; i++)
                    {
                        BotUnknown3.Add(packet.NextShort());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int UserId { get; set; }

        public string DisplayName { get; set; }

        public string Motto { get; set; }

        public string Figure { get; set; }

        public int VirtualId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public double Z { get; set; }

        public int Unknown0 { get; set; }

        public RoomUserType Unknown1 { get; set; }

        // User data.

        public string UserGender { get; set; }

        public int UserUnknown0 { get; set; }

        public int UserUnknown1 { get; set; }

        public string UserUnknown2 { get; set; }

        public string UserUnknown3 { get; set; }

        public int UserUnknown4 { get; set; }

        public bool UserUnknown5 { get; set; }
        
        // Pet data.

        public int PetUnknown0 { get; set; }

        public int PetUnknown1 { get; set; }

        public string PetUnknown2 { get; set; }

        public int PetUnknown3 { get; set; }

        public bool PetUnknown4 { get; set; }

        public bool PetUnknown5 { get; set; }

        public bool PetUnknown6 { get; set; }

        public bool PetUnknown7 { get; set; }

        public bool PetUnknown8 { get; set; }

        public bool PetUnknown9 { get; set; }

        public int PetUnknown10 { get; set; }

        public string PetUnknown11 { get; set; }

        // Bot data.

        public string BotUnknown0 { get; set; }

        public int BotUnknown1 { get; set; }

        public string BotUnknown2 { get; set; }

        public List<short> BotUnknown3 { get; set; }
    }
}
