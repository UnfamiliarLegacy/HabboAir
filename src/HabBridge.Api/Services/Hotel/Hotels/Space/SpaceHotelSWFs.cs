using System.Threading.Tasks;
using HabBridge.Hotels;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.Services.Hotel.Hotels.Space
{
    public class SpaceHotelSWFs : IHotelSWFs
    {
        public Task<IActionResult> GetExternalVariablesUrl()
        {
            return Task.FromResult((IActionResult) new RedirectResult(HotelManager.GetHotel(HotelType.Space).ExternalVariables));
        }

        public string GetExternalFlashTextsUrl()
        {
            throw new System.NotImplementedException();
        }

        public string GetProductDataUrl()
        {
            throw new System.NotImplementedException();
        }

        public string GetFurniDataUrl()
        {
            throw new System.NotImplementedException();
        }

        public string GetHabboImagingAvatar(string figure, int headDirection = 2)
        {
            return $"https://habbo.city/habbo-imaging/avatarimage?figure={figure}&head_direction={headDirection}";
        }
    }
}
