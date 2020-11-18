using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel.Data;

namespace HabBridge.Api.Services.Hotel.Hotels
{
    public interface IHotelHttp
    {
        /// <summary>
        ///     Authenticates with the retrohotel.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> AuthenticateAsync(string username, string password);

        /// <summary>
        ///     Checks if the current instance is authenticated.
        /// </summary>
        /// <returns></returns>
        Task<bool> IsAuthenticatedAsync();

        /// <summary>
        ///     Retrieves all avatars bound to the signed in user.
        /// </summary>
        /// <returns></returns>
        Task<HotelAvatar[]> GetAvatarsAsync();

        /// <summary>
        ///     Select an avatar on the hotel website if required.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns>Return null if not found.</returns>
        Task<HotelAvatar> SelectAvatarAsync(int avatarId);

        Task<HotelAvatar> GetCurrentAvatarAsync();

        Task<string> GetClientSsoTokenAsync();

        /// <summary>
        ///     Clears all cookies from the associated <see cref="System.Net.CookieContainer"/>.
        /// </summary>
        void ClearCookies();
    }
}
