using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HabBridge.Api.V1.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly SessionManagerService _sessionManager;

        public SessionMiddleware(
            RequestDelegate next,
            SessionManagerService sessionManager)
        {
            _next = next;
            _sessionManager = sessionManager;
        }

        public async Task Invoke(
            HttpContext ctx,
            HotelService hotelService,
            SessionCookieService sessionCookie,
            RequestData requestData)
        {
            // Get or set a session.
            if (sessionCookie.TryGetSessionHash(out var sessionHash) && _sessionManager.TryGetSession(sessionHash, out var session))
            {
                requestData.Session = session;

                if (!requestData.Session.Verify(hotelService.CurrentHotel))
                {
                    requestData.Session = await _sessionManager.CreateSessionAsync(hotelService.CurrentHotel);
                    
                    // Send session back to client.
                    sessionCookie.SetSessionHash(requestData.Session.Id.Hash);
                }
            }
            else
            {
                requestData.Session = await _sessionManager.CreateSessionAsync(hotelService.CurrentHotel);

                // Send session back to client.
                sessionCookie.SetSessionHash(requestData.Session.Id.Hash);
            }

            // Verify session.
            // if (!requestData.Session.Verify(hotelService.CurrentHotel))
            // {
            //     ctx.Response.ContentType = "application/json";
            //     ctx.Response.StatusCode = 403;
            //
            //     await ctx.Response.WriteAsync(JsonConvert.SerializeObject(new
            //     {
            //         Error = "Invalid session."
            //     }));
            //
            //     return;
            // }

            // Continue executing.
            await _next(ctx);

            // Save any changes.
            if (requestData.Session != null &&
                requestData.Session.SaveChanges)
            {
                await _sessionManager.SaveSession(requestData.Session);
            }
        }
    }
}
