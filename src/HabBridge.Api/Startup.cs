using System;
using System.Net.Http;
using HabBridge.Api.Config;
using HabBridge.Api.Services.Cache;
using HabBridge.Api.Services.Hotel;
using HabBridge.Api.Services.Hotel.Hotels.Leet;
using HabBridge.Api.Services.Sessions;
using HabBridge.Api.V1.Data;
using HabBridge.Api.V1.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HabBridge.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.AddOptions();
            
            // Add HttpClient.
            services.AddHttpClient();

            // Make HttpContext available.
            services.AddHttpContextAccessor();

            // Used to create instances related to hotels.
            services.AddHotelService();

            // Caching.
            services.AddScoped<CacheService>();

            // Manages sessions.
            services.AddScoped<SessionService>();
            services.AddScoped<SessionCookieService>();
            services.AddSingleton<SessionManagerService>();

            // Add MVC.
            services.AddControllers();

            // Adds redis caching for persistency.
            services.AddDistributedRedisCache(options =>
            {
                var cacheOptions = Configuration.GetSection("Cache").Get<CacheConfig>();

                options.InstanceName = "Local";
                options.Configuration = cacheOptions.RedisAddress;
            });

            // Holds data for the current request.
            services.AddScoped<RequestData>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/v1"), builder =>
            {
                builder.UseMiddleware<HotelMiddleware>();
                builder.UseMiddleware<SessionMiddleware>();
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });
        }
    }
}
