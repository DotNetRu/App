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
    public class VkontakteController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly VkontakteSettings vkontakteSettings;

        private readonly Dictionary<string, byte> defaultCommunities = new Dictionary<string, byte> { { "DotNetRu", 10 } };

        public VkontakteController(
            ILogger<DiagnosticsController> logger,
            VkontakteSettings vkontakteSettings)
        {
            this.logger = logger;
            this.vkontakteSettings = vkontakteSettings;
        }

        [HttpPost]
        [Route("subscriptionVkontaktePosts")]
        public async Task<IActionResult> GetOriginalPostsBySubscriptions(
            [FromBody] Dictionary<string, byte> communities = null
        )
        {
            return await GetOriginalPosts(communities);
        }

        [HttpGet]
        [Route("VkontaktePosts")]
        public async Task<IActionResult> GetAllOriginalPosts()
        {
            return await GetOriginalPosts(defaultCommunities);
        }

        private async Task<IActionResult> GetOriginalPosts(
            // key - имя комьюнти - "dotnetru"
            // value - число загружаемых постов (от 0 до 100)
            Dictionary<string, byte> communities
            )
        {
            try
            {
                communities ??= defaultCommunities;

                var allPosts = await CacheHelper.PostsCache.GetOrCreateAsync("vkontaktePosts",
                    async () => await VkontakteService.GetAsync(this.vkontakteSettings,
                        communities));

                var posts = new List<ISocialPost>();
                foreach (var community in communities)
                    posts.AddRange(allPosts.Where(x =>
                            x.CommunityGroupId.Equals(community.Key, StringComparison.InvariantCultureIgnoreCase))
                        .Take(community.Value));

                var json = JsonConvert.SerializeObject(posts);

                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled error while getting original vkontakte posts");
                return new ObjectResult(e)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
