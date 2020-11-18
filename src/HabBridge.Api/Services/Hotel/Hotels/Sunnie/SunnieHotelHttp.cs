using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel.Data;

namespace HabBridge.Api.Services.Hotel.Hotels.Sunnie
{
    public class SunnieHotelHttp : IHotelHttp
    {
        public Task<bool> AuthenticateAsync(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<HotelAvatar[]> GetAvatarsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<HotelAvatar> SelectAvatarAsync(int avatarId)
        {
            throw new System.NotImplementedException();
        }

        public Task<HotelAvatar> GetCurrentAvatarAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetClientSsoTokenAsync()
        {
            throw new System.NotImplementedException();
        }

        public void ClearCookies()
        {
            throw new System.NotImplementedException();
        }
    }
}
