using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AzureService;
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
                var cacheKey = "vkontaktePosts";
                if (communities == null || !communities.Any())
                    communities = defaultCommunities;

                cacheKey += string.Join(";",
                    communities.OrderBy(x => x.Key).Select(x => $"{x.Key}:{x.Value}").ToArray());

                var posts = await CacheHelper.PostsCache.GetOrCreateAsync(cacheKey,
                    async () => await VkontakteService.GetAsync(this.vkontakteSettings,
                        communities));

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
