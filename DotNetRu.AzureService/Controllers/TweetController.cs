using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AzureService;
using DotNetRu.Clients.UI;
using DotNetRu.Models.Social;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DotNetRu.Azure
{
    [Route("")]
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

        [HttpPost]
        [Route("subscriptionTweets")]
        public async Task<IActionResult> GetOriginalTweetsBySubscriptions(
            [FromBody]List<string> communities = null
        )
        {
            return await GetOriginalTweets(communities);
        }

        [HttpGet]
        [Route("tweets")]
        public async Task<IActionResult> GetAllOriginalTweets()
        {
            return await GetOriginalTweets(AppConfig.GetConfig().CommunityGroups
                .Where(x => x.IsSelected && x.Type == SocialMediaType.Twitter).Select(x => x.CommunityName).ToList());
        }

        private async Task<IActionResult> GetOriginalTweets(
            List<string> communities
        )
        {
            try
            {
                var cacheKey = "tweets";
                if (communities == null || !communities.Any())
                    communities = AppConfig.GetConfig().CommunityGroups.Where(x => x.IsSelected && x.Type == SocialMediaType.Twitter).Select(x => x.CommunityName).ToList();

                cacheKey += string.Join(";", communities.OrderBy(x => x).ToArray());

                var tweets = await CacheHelper.PostsCache.GetOrCreateAsync(cacheKey,
                    async () => await TweetService.GetAsync(this.tweetSettings,
                        communities));

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
