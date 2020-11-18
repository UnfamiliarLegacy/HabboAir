using System;
using System.Linq;
using System.Threading.Tasks;
using HabBridge.Api.Services.Cache;
using HabBridge.Api.Services.Hotel.Hotels;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Requests;
using HabBridge.Api.V1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers.Api
{
    [Route("v1/{hotel}/api/user")]
    public class UserController : ControllerBase
    {
        private readonly CacheService _cacheService;

        private readonly SessionService _sessionService;

        private readonly IHotelHttp _hotelHttp;

        public UserController(CacheService cacheService, SessionService sessionService, IHotelHttp hotelHttp)
        {
            _cacheService = cacheService;
            _sessionService = sessionService;
            _hotelHttp = hotelHttp;
        }

        [HttpGet("avatars")]
        public async Task<ActionResult> ListAvatarsAsync()
        {
            var avatars = await _hotelHttp.GetAvatarsAsync();

            await _cacheService.CacheAvatarsAsync(avatars);
            await _cacheService.SetAvatarsCachedForSessionAsync();

            return Ok(avatars.Select(item => new AvatarResponse
            {
                UniqueId = $"{item.AvatarId}",
                Name = item.Name,
                FigureString = item.Figure,
                Motto = item.Motto,
                BuildersClubMember = false,
                HabboClubMember = true,
                LastWebAccess = DateTimeOffset.UtcNow,
                CreationTime = DateTimeOffset.UtcNow,
                Banned = false
            }));
        }

        [HttpPost("avatars/select")]
        public async Task<ActionResult> SelectAvatar([FromBody] AvatarSelectRequest request)
        {
            if (!int.TryParse(request.UniqueId, out var uniqueIdInt))
            {
                return BadRequest();
            }

            var avatar = await _hotelHttp.SelectAvatarAsync(uniqueIdInt);
            if (avatar == null)
            {
                await _sessionService.LogOutAsync();

                return Unauthorized();
            }

            return Ok(new AvatarSelectResponse
            {
                UniqueId = $"{avatar.AvatarId}",
                Name = avatar.Name,
                FigureString = avatar.Figure,
                Motto = avatar.Motto,
                BuildersClubMember = false,
                HabboClubMember = true,
                LastWebAccess = DateTimeOffset.UtcNow,
                CreationTime = DateTimeOffset.UtcNow
            });
        }
    }
}
