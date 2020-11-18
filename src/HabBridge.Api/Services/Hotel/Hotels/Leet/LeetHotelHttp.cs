using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HabBridge.Api.Extensions;
using HabBridge.Api.Services.Cache;
using HabBridge.Api.Services.Hotel.Data;
using HabBridge.Api.V1.Data;
using Microsoft.Extensions.Logging;

namespace HabBridge.Api.Services.Hotel.Hotels.Leet
{
    public class LeetHotelHttp : IHotelHttp, IDisposable
    {
        private static readonly Regex PatternSso = new Regex("\"sso.ticket\": \"(.*?)\"", RegexOptions.Compiled);
        
        private readonly ILogger<LeetHotelHttp> _logger;
        private readonly CacheService _cacheService;
        private readonly CookieContainer _cookies;
        private readonly HttpClient _client;
        private readonly HtmlParser _parser;

        public LeetHotelHttp(ILogger<LeetHotelHttp> logger, CacheService cacheService, RequestData requestData)
        {
            _logger = logger;
            _cacheService = cacheService;
            _cookies = requestData.Session.Cookies;
            
            _client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = true,
                CookieContainer = _cookies,
                AutomaticDecompression = DecompressionMethods.All
            })
            {
                BaseAddress = new Uri("https://www.leet.ws"),
                DefaultRequestHeaders =
                {
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                    "Chrome/60.0.3112.113 " +
                                    "Safari/537.36" }
                }
            };
            
            _parser = new HtmlParser(new HtmlParserOptions
            {
                IsEmbedded = false,
                IsScripting = false,
                IsStrictMode = false
            });
        }
        
        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            using var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("credentials.username", username),
                new KeyValuePair<string, string>("credentials.password", password),
            });
            
            using var res = await _client.PostAsync("/account/submit", content);

            if (res.StatusCode == HttpStatusCode.Redirect)
            {
                var location = res.Headers.Location.ToString();
                if (location.StartsWith("/me"))
                {
                    return true;
                }

                return false;
            }
            
            throw new HotelHttpException($"Unexpected response code {res.StatusCode} on login for {username}.");
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<HotelAvatar[]> GetAvatarsAsync()
        {
            var data = await _client.GetStringAsync("/me");
            var document = await _parser.ParseDocumentAsync(data);
            
            // Name.
            var name = document.QuerySelector(".username div").Text();
            
            // Figure.
            var avatarImgSrc = document.QuerySelector("div#avatar").GetAttribute("style");
            var figureStart = avatarImgSrc.IndexOf("=", StringComparison.InvariantCultureIgnoreCase);
            var figureEnd = avatarImgSrc.IndexOf("&", StringComparison.InvariantCultureIgnoreCase);

            var figure = avatarImgSrc.Substring(figureStart + 1, figureEnd - figureStart - 1);
            
            return new[]
            {
                new HotelAvatar
                {
                    AvatarId = 0,
                    Name = name,
                    Motto = string.Empty,
                    Figure = figure
                }
            };
        }

        public async Task<HotelAvatar> SelectAvatarAsync(int avatarId)
        {
            return _cacheService.TryGetAvatarById(0, out var avatar)
                ? avatar
                : (await GetAvatarsAsync())[0];
        }

        public async Task<HotelAvatar> GetCurrentAvatarAsync()
        {
            return _cacheService.TryGetAvatarById(0, out var avatar) 
                ? avatar 
                : (await GetAvatarsAsync())[0];
        }

        public async Task<string> GetClientSsoTokenAsync()
        {
            using var res = await _client.GetAsync("/client");

            var data = await res.Content.ReadAsStringAsync();
            
            if (res.StatusCode == HttpStatusCode.Redirect)
            {
                return null;
            }

            if (res.StatusCode != HttpStatusCode.OK)
            {
                throw new HotelHttpException($"Unexpected response code {res.StatusCode} on client.", data);
            }
            
            if (data.Contains("VPN Geblokkeerd"))
            {
                throw new HotelHttpException("Proxy was blocked.", data);
            }
            
            if (data.Contains("Zet je VPN uit!"))
            {
                throw new HotelHttpException("Proxy was blocked.", data);
            }

            return PatternSso.Match(data).Groups[1].Value;
        }

        public void ClearCookies()
        {
            _cookies.ClearAll();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}