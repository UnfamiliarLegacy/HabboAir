using System;
using System.Collections.Generic;
using HabBridge.Api.Services.Hotel.Hotels.Leet;
using HabBridge.Api.Services.Hotel.Hotels.Local;
using HabBridge.Api.Services.Hotel.Hotels.Space;
using HabBridge.Api.Services.Hotel.Hotels.Sunnie;
using HabBridge.Hotels;

namespace HabBridge.Api.Services.Hotel
{
    /// <summary>
    ///     Scoped dependency.
    /// </summary>
    public class HotelService
    {
        private static readonly Dictionary<HotelType, HotelClassTypes> Hotels = new Dictionary<HotelType, HotelClassTypes>
        {
            { HotelType.Local, new HotelClassTypes(typeof(LocalHotelHttp), typeof(LocalHotelSWFs)) }, 
            { HotelType.Sunnie, new HotelClassTypes(typeof(SunnieHotelHttp), typeof(SunnieHotelSWFs)) },
            { HotelType.Space, new HotelClassTypes(typeof(SpaceHotelHttp), typeof(SpaceHotelSWFs)) },
            { HotelType.Leet, new HotelClassTypes(typeof(LeetHotelHttp), typeof(LeetHotelSWFs)) },
        };
        
        public HotelType CurrentHotel { get; private set; }

        public HotelClassTypes ClassTypes { get; private set; }

        public bool IsConfigured { get; private set; }

        public bool TryConfigure(string hotelCode)
        {
            if (Enum.TryParse<HotelType>(hotelCode, true, out var hotel) && Hotels.ContainsKey(hotel))
            {
                CurrentHotel = hotel;
                ClassTypes = Hotels[hotel];
                IsConfigured = true;

                return true;
            }

            return false;
        }
    }
}
