using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AzureService;
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

        private readonly List<string> defaultCommunities = new List<string> {"DotNetRu", "SpbDotNet"};

        public TweetController(
            ILogger<DiagnosticsController> logger,
            TweetSettings tweetSettings)
        {
            this.logger = logger;
            this.tweetSettings = tweetSettings;
        }

        [HttpGet]
        [Route("tweets")]
        public async Task<IActionResult> GetTweets([FromQuery] List<string> communities = null)
        {
            return await GetOriginalTweets(communities ?? defaultCommunities);
        }

        private async Task<IActionResult> GetOriginalTweets(List<string> communities)
        {
            try
            {
                var allTweets = await CacheHelper.PostsCache.GetOrCreateAsync("tweets",
                    async () => await TweetService.GetAsync(this.tweetSettings, communities));

                var tweets = new List<ISocialPost>();
                foreach (var community in communities)
                {
                    tweets.AddRange(allTweets.Where(x =>
                        x.CommunityGroupId.Equals(community, StringComparison.InvariantCultureIgnoreCase)));
                }

                var json = JsonConvert.SerializeObject(tweets);

                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled error while getting tweets");
                return new ObjectResult(e)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
