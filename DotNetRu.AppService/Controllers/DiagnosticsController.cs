using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetRu.AzureService;

namespace DotNetRu.Azure
{
    [Route("diagnostics")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly TweetSettings tweetSettings;
        private readonly PushSettings pushSettings;
        private readonly VkontakteSettings vkontakteSettings;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            TweetSettings tweetSettings,
            VkontakteSettings vkontakteSettings,
            PushSettings pushSettings)
        {
            this.logger = logger;
            this.tweetSettings = tweetSettings;
            this.vkontakteSettings = vkontakteSettings;
            this.pushSettings = pushSettings;
        }

        [HttpGet]
        [Route("settings")]
        public IActionResult Settings()
        {
            return new ObjectResult(new
            {
                TweetSettings = tweetSettings,
                VkontakteSettings = vkontakteSettings,
                pushSettings = pushSettings
            });
        }
    }
}
