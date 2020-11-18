using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HabBridge.Hotels;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.Services.Hotel.Hotels.Leet
{
    public class LeetHotelSWFs : IHotelSWFs
    {
        private readonly HttpClient _client;
        
        public LeetHotelSWFs(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
        }
        
        public async Task<IActionResult> GetExternalVariablesUrl()
        {
            // TODO: Check if external_vars or flash texts fucks up leet.
            // return (IActionResult) new RedirectResult("https://cdn-dev.moonhotel.nl/gamedata/external_variables.txt");
            
            // Modify:
            // - flash.client.url
            // - furnidata.load.url
            // - productdata.load.url
            // - external.texts.txt
            // - external.variables.txt
            // - external.figurepartlist.txt
            // - external.override.variables.txt
            // - external.override.texts.txt

            // Parse original external variables.
            var dataUrl = HotelManager.GetHotel(HotelType.Leet).ExternalVariables;
            var dataStr = await _client.GetStringAsync(dataUrl);
            var dataParsed = dataStr.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var dataDict = new SortedDictionary<string, string>();
            
            foreach (var line in dataParsed)
            {
                var parts = line.Split('=', 2);
                dataDict[parts[0]] = parts[1];
            }
            
            // Modify.
            dataDict.Remove("external.texts.txt");
            dataDict.Remove("external.variables.txt");
            dataDict["furnidata.load.url"] = "https://www.leet.ws/gamedata/leet_furni.txt?v=467";
            dataDict["productdata.load.url"] = "https://www.leet.ws/gamedata/leet_produ.txt?v=467";
            dataDict["external.figurepartlist.txt"] = "https://images.leet.ws/library/gamedata/figuredata.xml?v=467";
            dataDict["external.override.variables.txt"] = "https://images.leet.ws/library/gamedata/leet_override_vars.txt";
            dataDict["external.override.texts.txt"] = "https://images.leet.ws/library/gamedata/leet_override_texts.txt";
            dataDict["flash.client.url"] = "https://images.leet.ws/library/";

            dataDict["news.url"] = "${url.prefix}/news";
            
            // Get output.
            var builder = new StringBuilder();
            
            foreach (var (key, value) in dataDict)
            {
                builder.AppendFormat("{0}={1}\r\n", key, value);
            }
            
            return new OkObjectResult(builder.ToString());
        }

        public string GetExternalFlashTextsUrl()
        {
            // return "https://cdn-dev.moonhotel.nl/gamedata/external_flash_texts.txt";
            return HotelManager.GetHotel(HotelType.Leet).ExternalFlashTexts;
        }

        public string GetProductDataUrl()
        {
            throw new NotImplementedException();
        }

        public string GetFurniDataUrl()
        {
            throw new NotImplementedException();
        }

        public string GetHabboImagingAvatar(string figure, int headDirection = 2)
        {
            return $"https://cdn.leet.ws/leet-imaging/avatarimage?figure={figure}&head_direction={headDirection}";
        }
    }
}