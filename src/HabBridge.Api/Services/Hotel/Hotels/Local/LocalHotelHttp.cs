using System;
using System.Net;
using System.Threading.Tasks;
using HabBridge.Api.Extensions;
using HabBridge.Api.Services.Hotel.Data;
using HabBridge.Api.V1.Data;

namespace HabBridge.Api.Services.Hotel.Hotels.Local
{
    public class LocalHotelHttp : IHotelHttp
    {
        private const string LocalHotelUrl = "localhotel.me";
        private const string LocalHotelAuthCookie = "AUTH";

        private readonly CookieContainer _cookies;

        public LocalHotelHttp(RequestData requestData)
        {
            _cookies = requestData.Session.Cookies;
        }

        public Task<bool> AuthenticateAsync(string username, string password)
        {
            // if (username.Equals("test", StringComparison.InvariantCultureIgnoreCase) &&
            //     password.Equals("test", StringComparison.InvariantCultureIgnoreCase))
            // {
                _cookies.Add(new Uri($"https://{LocalHotelUrl}"), new Cookie(LocalHotelAuthCookie, "ok"));
                
                return Task.FromResult(true);
            // }

            // return Task.FromResult(false);
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            var sessionCookie = _cookies.GetCookie(LocalHotelAuthCookie, LocalHotelUrl);

            return Task.FromResult(sessionCookie != null && !sessionCookie.Expired);
        }

        public Task<HotelAvatar[]> GetAvatarsAsync()
        {
            return Task.FromResult(new []
            {
                new HotelAvatar
                {
                    AvatarId = 1,
                    Name = "Test",
                    Motto = "Test motto",
                    Figure = "hd-180-3.hr-828-61.ha-1015-110.ch-215-82.lg-270-64.sh-290-62"
                },
                new HotelAvatar
                {
                    AvatarId = 2,
                    Name = "Bot",
                    Motto = "Bot motto",
                    Figure = "hd-180-1.hr-831-61.ch-255-73.lg-280-110.sh-295-86"
                }
            });
        }

        public Task<HotelAvatar> SelectAvatarAsync(int avatarId)
        {
            var avatars = GetAvatarsAsync().Result;

            foreach (var avatar in avatars)
            {
                if (avatar.AvatarId == avatarId)
                {
                    return Task.FromResult(avatar);
                }
            }

            return null;
        }

        public Task<HotelAvatar> GetCurrentAvatarAsync()
        {
            return Task.FromResult(GetAvatarsAsync().Result[0]);
        }

        public Task<string> GetClientSsoTokenAsync()
        {
            return Task.FromResult("DEV");
        }

        public void ClearCookies()
        {
            _cookies.ClearAll();
        }
    }
}
