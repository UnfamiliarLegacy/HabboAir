using HabBridge.Api.V1.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HabBridge.Api.V1.Controllers.Api
{
    [Route("v1/{hotel}/api/log")]
    public class LogController : ControllerBase
    {
        private readonly ILogger _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpPost("loginstep")]
        public ActionResult LogLoginStep([FromBody] LogLoginStepRequest request)
        {
            _logger.LogWarning($"Login Step: {request.Step}");

            return StatusCode(204);
        }

        [HttpPost("error")]
        public ActionResult LogError([FromBody] LogErrorRequest request)
        {
            _logger.LogError($"Client Error: {request.Message}");

            return StatusCode(204);
        }

        [HttpPost("crash")]
        public ActionResult LogError([FromBody] LogCrashRequest request)
        {
            _logger.LogError($"Client Crash: {request.MessageDecoded}");

            return StatusCode(204);
        }
    }
}
