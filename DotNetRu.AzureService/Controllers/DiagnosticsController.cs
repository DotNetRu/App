using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace DotNetRu.Azure
{
    [Route("diagnostics")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger logger;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Route("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                logger.LogInformation("Ping is requested");

                return new OkObjectResult("Success");
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled error while ping");
                return new ObjectResult(e) { 
                    StatusCode = StatusCodes.Status500InternalServerError 
                };
            }
        }
    }
}
