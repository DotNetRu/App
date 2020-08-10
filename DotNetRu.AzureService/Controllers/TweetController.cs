using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AzureService;
using DotNetRu.AzureService.Helpers;
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

        private readonly RealmSettings realmSettings;

        private readonly PushNotificationsManager pushNotificationsManager;

        private readonly List<string> defaultCommunities = new List<string> {"DotNetRu", "SpbDotNet"};

        public TweetController(
            ILogger<DiagnosticsController> logger,
            TweetSettings tweetSettings,
            RealmSettings realmSettings,
            PushNotificationsManager pushNotificationsManager)
        {
            this.logger = logger;
            this.tweetSettings = tweetSettings;
            this.realmSettings = realmSettings;
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
            return await GetOriginalTweets(defaultCommunities);
        }

        private async Task<IActionResult> GetOriginalTweets(
            List<string> communities
        )
        {
            try
            {
                communities ??= defaultCommunities;

                var allCommunities = (await realmSettings.GetAllCommunitiesAsync(SocialMediaType.Twitter))
                    .Select(x => x.Community.Name).ToList();

                var allTweets = await CacheHelper.PostsCache.GetOrCreateAsync("tweets",
                    async () => await TweetService.GetAsync(this.tweetSettings,
                        allCommunities));

                var tweets = new List<ISocialPost>();
                foreach (var community in communities)
                    tweets.AddRange(allTweets.Where(x =>
                        x.CommunityGroupId.Equals(community, StringComparison.InvariantCultureIgnoreCase)));

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
