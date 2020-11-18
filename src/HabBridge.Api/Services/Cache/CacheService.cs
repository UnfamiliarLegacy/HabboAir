using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel.Data;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Data;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace HabBridge.Api.Services.Cache
{
    public class CacheService
    {
        private readonly IDistributedCache _cache;

        private readonly Session _session;

        public CacheService(IDistributedCache cache, RequestData requestData)
        {
            _cache = cache;
            _session = requestData.Session;
        }

        public async Task CacheAvatarAsync(HotelAvatar avatar)
        {
            await _cache.SetStringAsync($"avatar_{_session.Hotel}_{avatar.Name}", JsonConvert.SerializeObject(avatar));
            await _cache.SetStringAsync($"avatar_{_session.Hotel}_id_{avatar.AvatarId}", JsonConvert.SerializeObject(avatar));
        }

        public async Task CacheAvatarsAsync(IEnumerable<HotelAvatar> avatars)
        {
            await Task.WhenAll(avatars.Select(CacheAvatarAsync));
        }

        /// <summary>
        ///     Puts in the cache that the avatars of the current session
        ///     have been properly cached.
        /// </summary>
        /// <returns></returns>
        public async Task SetAvatarsCachedForSessionAsync()
        {
            await _cache.SetAsync($"avatar_session-{_session.Id.Hash}", new byte[] {0xFF});
        }

        /// <summary>
        ///     Checks if the cache contains a value that the avatars
        ///     of the current session have been cached.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsAvatarsCachedForSessionAsync()
        {
            var bytes = await _cache.GetAsync($"avatar_session-{_session.Id.Hash}");

            return bytes != null && bytes.Length != 0;
        }

        public bool TryGetAvatar(string username, out HotelAvatar avatar)
        {
            var raw = _cache.GetString($"avatar_{_session.Hotel}_{username}");

            if (string.IsNullOrEmpty(raw))
            {
                avatar = null;
                return false;
            }

            avatar = JsonConvert.DeserializeObject<HotelAvatar>(raw);
            return true;
        }

        public bool TryGetAvatarById(int avatarId, out HotelAvatar avatar)
        {
            var raw = _cache.GetString($"avatar_{_session.Hotel}_id_{avatarId}");

            if (string.IsNullOrEmpty(raw))
            {
                avatar = null;
                return false;
            }

            avatar = JsonConvert.DeserializeObject<HotelAvatar>(raw);
            return true;
        }
    }
}
