using System;
using System.Linq;
using HabBridge.Api.Services.Hotel.Hotels;
using Microsoft.Extensions.DependencyInjection;

namespace HabBridge.Api.Services.Hotel
{
    public static class HotelServiceExtensions
    {
        public static void AddHotelService(this IServiceCollection services)
        {
            services.AddScoped<HotelService>();

            services.AddScoped(ctx =>
            {
                var hotelService = ctx.GetRequiredService<HotelService>();
                if (!hotelService.IsConfigured)
                {
                    throw new Exception("HotelService has not been configured yet.");
                }

                return (IHotelHttp) ctx.GetRequiredService(hotelService.ClassTypes.HotelHttp);
            });

            services.AddScoped(ctx =>
            {
                var hotelService = ctx.GetRequiredService<HotelService>();
                if (!hotelService.IsConfigured)
                {
                    throw new Exception("HotelService has not been configured yet.");
                }

                return (IHotelSWFs) ctx.GetRequiredService(hotelService.ClassTypes.HotelSWFs);
            });

            // Auto register interfaces.
            var hotelHttpType = typeof(IHotelHttp);
            var hotelSWFsType = typeof(IHotelSWFs);
            var scopedTypes = typeof(HotelServiceExtensions).Assembly
                .GetTypes()
                .Where(p => !p.IsInterface && (hotelHttpType.IsAssignableFrom(p) || hotelSWFsType.IsAssignableFrom(p)));

            foreach (var scopedType in scopedTypes)
            {
                services.AddScoped(scopedType);
            }
        }
    }
}