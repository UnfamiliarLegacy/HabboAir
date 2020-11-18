using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers.Api.Public
{
    [Route("v1/{hotel}/api/public/info")]
    public class InfoController : ControllerBase
    {
        [HttpGet("hello")]
        public ActionResult HelloWorld()
        {
            return Ok(new
            {
                Message = "hello world"
            });
        }
    }
}
