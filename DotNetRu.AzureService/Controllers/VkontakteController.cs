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

        public VkontakteController(
            ILogger<DiagnosticsController> logger,
            VkontakteSettings vkontakteSettings)
        {
            this.logger = logger;
            this.vkontakteSettings = vkontakteSettings;
        }

        [HttpGet]
        [Route("VkontaktePosts")]
        public async Task<IActionResult> GetOriginalPosts(
            // key - имя комьюнти - "dotnetru"
            // value - число загружаемых постов (от 0 до 100)
            IDictionary<string, byte> communities = null
            )
        {
            try
            {
                var cacheKey = "vkontaktePosts";
                if (communities != null)
                    cacheKey += string.Join(";", communities.Select(x => $"{x.Key}:{x.Value}").ToArray());

                var posts = await CacheHelper.PostsCache.GetOrCreateAsync(cacheKey, async () => await VkontakteService.GetAsync(this.vkontakteSettings));

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
