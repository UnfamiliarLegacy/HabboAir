using System;
using System.Linq;
using System.Threading.Tasks;
using HabBridge.Api.Services.Cache;
using HabBridge.Api.Services.Hotel.Hotels;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Data;
using HabBridge.Api.V1.Requests;
using HabBridge.Api.V1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers.Api.Public
{
    [Route("v1/{hotel}/api/public/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IHotelHttp _hotelHttp;

        private readonly CacheService _cacheService;

        private readonly Session _session;

        public AuthenticationController(IHotelHttp hotelHttp, RequestData data, CacheService cacheService)
        {
            _hotelHttp = hotelHttp;
            _cacheService = cacheService;
            _session = data.Session;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return StatusCode(401, new LoginErrorResponse
                {
                    Message = LoginErrorCode.InvalidPassword,
                    Captcha = false
                });
            }

            // Fresh cookies.
            _hotelHttp.ClearCookies();

            // Try to authenticate at sunnieday.
            var authResult = await _hotelHttp.AuthenticateAsync(request.Email, request.Password);
            if (authResult == false)
            {
                return StatusCode(401, new LoginErrorResponse
                {
                    Message = LoginErrorCode.InvalidPassword,
                    Captcha = false
                });
            }

            // Set to authenticated in session.
            _session.Authenticated = true;
            _session.SaveChanges = true;

            // Load avatars.
            var avatars = await _hotelHttp.GetAvatarsAsync();

            // Cache avatars.
            await _cacheService.CacheAvatarsAsync(avatars);

            var avatar = avatars.First();

            // Select first avatar.
            await _hotelHttp.SelectAvatarAsync(avatar.AvatarId);

            // Get avatar information.
            var me = await _hotelHttp.GetCurrentAvatarAsync();

            // Fetch user data.
            return StatusCode(200, new LoginResponse
            {
                UniqueId = $"{avatar.AvatarId}",
                Name = me.Name,
                FigureString = me.Figure,
                Motto = me.Motto,
                BuildersClubMember = false, // ???
                HabboClubMember = true,
                LastWebAccess = DateTimeOffset.UtcNow,
                CreationTime = DateTimeOffset.UtcNow,
                SessionLogId = 1337,
                LoginLogId = 7331,
                Email = string.Empty, // TODO: Set?
                IdentityId = avatar.AvatarId,
                EmailVerified = true,
                IdentityVerified = true,
                IdentityType = "HABBO",
                Trusted = true,
                Force = new[]
                {
                    "NONE"
                },
                AccountId = 1, // ??? TODO
                Country = "nl",
                Traits = new[]
                {
                    "USER"
                },
                Partner = "NO_PARTNER"
            });
        }
    }
}
