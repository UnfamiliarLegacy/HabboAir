using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using HabBridge.Api.Extensions;
using HabBridge.Api.Services.Cache;
using HabBridge.Api.Services.Hotel.Data;
using HabBridge.Api.V1.Data;

namespace HabBridge.Api.Services.Hotel.Hotels.Space
{
    public class SpaceHotelHttp : IHotelHttp
    {
        private readonly CacheService _cacheService;
        private readonly CookieContainer _cookies;
        private readonly HttpClient _client;
        private readonly HtmlParser _parser;

        public SpaceHotelHttp(CacheService cacheService, RequestData requestData)
        {
            _cacheService = cacheService;
            _cookies = requestData.Session.Cookies;

            _client = new HttpClient(new SocketsHttpHandler
            {
                AllowAutoRedirect = false,
                UseCookies = true,
                CookieContainer = _cookies,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            {
                BaseAddress = new Uri("https://spacehotell.nl/"),
                DefaultRequestHeaders =
                {
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36" }
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
            // Cookie validation.
            await _client.GetAsync("/");

            // Login.
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (var result = await _client.PostAsync("/account/submit", requestContent))
            {
                if ((int) result.StatusCode == 302)
                {
                    var location = result.Headers.Location;
                    if (location != null)
                    {
                        return location.ToString().Contains("security_check");
                    }
                }
                
                return false;
            }
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<HotelAvatar[]> GetAvatarsAsync()
        {
            var data = await _client.GetStringAsync("/me");
            var document = await _parser.ParseDocumentAsync(data);

            var avatarImg = document.QuerySelector("#dox-plate img");
            var avatarImgSrc = avatarImg.GetAttribute("src");

            var name = avatarImg.GetAttribute("alt");
            var motto = document.QuerySelector("#motto-box .content").TextContent.Split('/', 2, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

            var figureStart = avatarImgSrc.IndexOf("=", StringComparison.InvariantCultureIgnoreCase);
            var figureEnd = avatarImgSrc.IndexOf("&", StringComparison.InvariantCultureIgnoreCase);

            var figure = avatarImgSrc.Substring(figureStart + 1, figureEnd - figureStart - 1);

            return new[]
            {
                new HotelAvatar
                {
                    AvatarId = 0,
                    Name = name,
                    Motto = motto,
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
            var client = await _client.GetStringAsync("/client");
            var ssoToken = Regex.Match(client, @"Client\.addVariable\(""sso.ticket"", ""(.*?)""\);", RegexOptions.Compiled).Groups[1].Value;

            return ssoToken;
        }

        public void ClearCookies()
        {
            _cookies.ClearAll();
        }
    }
}
