using System.Net;
using HabBridge.Api.Serializer;
using HabBridge.Hotels;
using Newtonsoft.Json;

namespace HabBridge.Api.Services.Sessions
{
    public class Session
    {
        public SessionId Id { get; set; }

        public HotelType Hotel { get; set; }

        public bool Authenticated { get; set; }

        [JsonConverter(typeof(CookieContainerSerializer))]
        public CookieContainer Cookies { get; set; }

        /// <summary>
        ///     Set to true if we have to (re)save the session (i.e. in redis).
        /// </summary>
        public bool SaveChanges { get; set; }

        public void Init()
        {
            Cookies = new CookieContainer();
            Id.Init();
        }

        public bool Verify(HotelType hotel)
        {
            return Hotel == hotel;
        }
    }
}
