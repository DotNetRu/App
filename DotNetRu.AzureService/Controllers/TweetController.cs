using System;
using System.Threading.Tasks;
using DotNetRu.AzureService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DotNetRu.Azure
{
    [Route("tweet")]
    public class TweetController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly TweetSettings tweetSettings;

        private readonly PushNotificationsManager pushNotificationsManager;

        public TweetController(
            ILogger<DiagnosticsController> logger,
            TweetSettings tweetSettings,
            PushNotificationsManager pushNotificationsManager)
        {
            this.logger = logger;
            this.tweetSettings = tweetSettings;
            this.pushNotificationsManager = pushNotificationsManager;
        }

        [HttpGet]
        [Route("get_original")]
        public async Task<IActionResult> GetOriginalTweets()
        {
            try
            {
                var tweets = await TweetService.GetAsync(tweetSettings);
                var json = JsonConvert.SerializeObject(tweets);

                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled error while getting original tweets");
                return new ObjectResult(e)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
