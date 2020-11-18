using System.Threading.Tasks;
using HabBridge.Api.Services.Hotel.Hotels;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers
{
    [Route("v1/{hotel}/gamedata")]
    public class GamedataController : ControllerBase
    {
        private readonly IHotelSWFs _swfs;

        public GamedataController(IHotelSWFs hotelSWFs)
        {
            _swfs = hotelSWFs;
        }

        [HttpGet("external_variables/1")]
        public async Task<IActionResult> GetLatestExternalVariables()
        {
            return await _swfs.GetExternalVariablesUrl();
        }

        [HttpGet("external_flash_texts/1")]
        public RedirectResult GetLatestExternalFlashTexts()
        {
            return Redirect(_swfs.GetExternalFlashTextsUrl());
        }

        // [HttpGet("productdata/1")]
        // public RedirectResult GetLatestProductData()
        // {
        //     return Redirect(_swfs.GetProductDataUrl());
        // }
        //
        // [HttpGet("furnidata/1")]
        // public RedirectResult GetLatestFurniData()
        // {
        //     return Redirect(_swfs.GetFurniDataUrl());
        // }
    }
}