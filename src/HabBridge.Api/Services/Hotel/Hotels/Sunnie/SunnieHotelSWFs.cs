using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.Services.Hotel.Hotels.Sunnie
{
    public class SunnieHotelSWFs : IHotelSWFs
    {
        public Task<IActionResult> GetExternalVariablesUrl()
        {
            throw new NotImplementedException();
        }

        public string GetExternalFlashTextsUrl()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
