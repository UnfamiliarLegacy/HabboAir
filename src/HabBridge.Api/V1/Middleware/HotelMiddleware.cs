using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel;
using HabBridge.Api.V1.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HabBridge.Api.V1.Middleware
{
    /// <summary>
    ///     Middleware responsible for parsing the hotel type from the path.
    /// </summary>
    public class HotelMiddleware
    {
        private readonly RequestDelegate _next;

        public HotelMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, HotelService hotelService)
        {
            if (httpContext.Request.Path.HasValue)
            {
                // Assuming the path is always in the form of /vX/hotel/...
                var path = httpContext.Request.Path.Value;
                var parts = path.Split('/', 4);
                if (parts.Length >= 3)
                {
                    var hotel = parts[2];
                    if (!string.IsNullOrEmpty(hotel) && hotelService.TryConfigure(hotel))
                    {
                        await _next(httpContext);

                        return;
                    }
                }
            }

            // The given hotel was not found.
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorMessage("The specified hotel could not be found")));
        }
    }
}
