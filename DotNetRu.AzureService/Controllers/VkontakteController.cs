using System;
using System.Collections.Generic;
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

        private static readonly CacheHelper.MemoryCacheWithPolicy<IList<ISocialPost>> PostsCache;

        static VkontakteController()
        {
            PostsCache = new CacheHelper.MemoryCacheWithPolicy<IList<ISocialPost>>();
            CacheHelper.SetExpirationInMinutes(5);
        }

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
            //IDictionary<string, int> dict
            )
        {
            try
            {
                //var cacheKey = string.Join(";", dict.Select(x => $"{x.Key}:{x.Value}").ToArray());

                // в дальнейшем key будет выбираться, исходя из параметров запроса пользователя (на какие сообщества он подписан)
                // или либо получать данные со всех сообществ, а отбирать их уже в клиенте
                var posts = await PostsCache.GetOrCreateAsync("vkontaktePosts", async () => await VkontakteService.GetAsync(this.vkontakteSettings));

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
