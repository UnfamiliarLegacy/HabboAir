using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.Services.Hotel.Hotels
{
    public interface IHotelSWFs
    {
        Task<IActionResult> GetExternalVariablesUrl();
        string GetExternalFlashTextsUrl();
        string GetProductDataUrl();
        string GetFurniDataUrl();
        string GetHabboImagingAvatar(string figure, int headDirection = 2);
    }
}
