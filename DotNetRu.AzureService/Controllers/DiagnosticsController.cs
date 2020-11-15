using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetRu.AzureService;

namespace DotNetRu.Azure
{
    [Route("diagnostics")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly RealmSettings realmSettings;
        private readonly TweetSettings tweetSettings;
        private readonly PushSettings pushSettings;
        private readonly VkontakteSettings vkontakteSettings;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            RealmSettings realmSettings,
            TweetSettings tweetSettings,
            VkontakteSettings vkontakteSettings,
            PushSettings pushSettings)
        {
            this.logger = logger;
            this.realmSettings = realmSettings;
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
                RealmSettings = realmSettings,
                TweetSettings = tweetSettings,
                VkontakteSettings = vkontakteSettings,
                pushSettings = pushSettings
            });
        }
    }
}
