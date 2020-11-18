using System.Threading.Tasks;
using HabBridge.Api.Services.Cache;
using HabBridge.Api.Services.Hotel.Hotels;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers
{
    [Route("v1/{hotel}/habbo-imaging")]
    public class HabboImagingController : ControllerBase
    {
        private readonly Session _session;

        private readonly CacheService _cacheService;

        private readonly IHotelHttp _hotelHttp;

        private readonly IHotelSWFs _hotelSwfs;

        public HabboImagingController(RequestData requestData, CacheService cacheService, IHotelHttp hotelHttp, IHotelSWFs hotelSwfs)
        {
            _session = requestData.Session;
            _cacheService = cacheService;
            _hotelHttp = hotelHttp;
            _hotelSwfs = hotelSwfs;
        }

        [HttpGet("avatarimage")]
        public async Task<ActionResult> Get([FromQuery(Name = "user")] string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest();
            }

            if (_session.Authenticated)
            {
                // Check if signed user his avatars are cached.
                if (!await _cacheService.IsAvatarsCachedForSessionAsync())
                {
                    var avatars = await _hotelHttp.GetAvatarsAsync();

                    // Store looks cache for re-use.
                    await _cacheService.CacheAvatarsAsync(avatars);
                    await _cacheService.SetAvatarsCachedForSessionAsync();
                }
            }

            // Check cache.
            if (_cacheService.TryGetAvatar(user, out var avatar))
            {
                return Redirect(_hotelSwfs.GetHabboImagingAvatar(avatar.Figure));
            }

            return NotFound("User could not be found.");
        }
    }
}
