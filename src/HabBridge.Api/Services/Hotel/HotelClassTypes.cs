using System;

namespace HabBridge.Api.Services.Hotel
{
    public class HotelClassTypes
    {
        public HotelClassTypes(Type HotelHttp, Type HotelSWFs)
        {
            this.HotelHttp = HotelHttp;
            this.HotelSWFs = HotelSWFs;
        }

        public Type HotelHttp { get; }

        public Type HotelSWFs { get; }
    }
}
