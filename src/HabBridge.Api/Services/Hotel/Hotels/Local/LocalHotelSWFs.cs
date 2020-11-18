using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.Services.Hotel.Hotels.Local
{
    public class LocalHotelSWFs : IHotelSWFs
    {
        public Task<IActionResult> GetExternalVariablesUrl()
        {
            // return "https://www.habbo.com/gamedata/external_variables/1";
            return Task.FromResult((IActionResult) new RedirectResult("https://cdn-dev.moonhotel.nl/gamedata/external_variables.txt"));
        }

        public string GetExternalFlashTextsUrl()
        {
            // return "https://www.habbo.com/gamedata/external_flash_texts/1";
            return "https://cdn-dev.moonhotel.nl/gamedata/external_flash_texts.txt";
        }

        public string GetProductDataUrl()
        {
            return "https://cdn-dev.moonhotel.nl/gamedata/productdata.txt";
        }

        public string GetFurniDataUrl()
        {
            return "https://cdn-dev.moonhotel.nl/gamedata/furnidata.xml";
        }

        public string GetHabboImagingAvatar(string figure, int headDirection = 2)
        {
            return $"https://www.habbo.nl/habbo-imaging/avatarimage?figure={figure}&head_direction={headDirection}";
        }
    }
}
