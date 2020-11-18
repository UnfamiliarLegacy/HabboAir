using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel.Hotels;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Data;
using HabBridge.Api.V1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers
{
    [Route("v1/{hotel}/api")]
    public class ApiController : ControllerBase
    {
        private readonly Session _session;

        private readonly IHotelHttp _hotelHttp;

        public ApiController(RequestData requestData, IHotelHttp hotelHttp)
        {
            _session = requestData.Session;
            _hotelHttp = hotelHttp;
        }

        [HttpGet("ssotoken")]
        public async Task<ActionResult> GetSsoToken()
        {
            if (!_session.Authenticated)
            {
                return Unauthorized();
            }

            return Ok(new SsoTokenResponse
            {
                SsoToken = await _hotelHttp.GetClientSsoTokenAsync()
            });
        }
    }
}
