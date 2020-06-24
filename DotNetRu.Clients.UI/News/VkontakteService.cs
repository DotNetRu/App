using System.Linq;
using DotNetRu.Models.Social;

namespace DotNetRu.Clients.Portable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.UI;
    using DotNetRu.Utils;
    using Flurl.Http;

    public static class VkontakteService
    {
        public static async Task<List<ISocialPost>> GetAsync()
        {
            try
            {
                var config = AppConfig.GetConfig();

                var vkontaktePosts = await config.VkontakteFunctionUrl
                    .GetJsonAsync<List<VkontaktePost>>();

                return vkontaktePosts.Cast<ISocialPost>().ToList();
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }

            return new List<ISocialPost>();
        }

        public static async Task<List<ISocialPost>> GetBySubscriptionsAsync(Dictionary<string, byte> communities)
        {
            try
            {
                var config = AppConfig.GetConfig();

                var vkontaktePosts = await config.SubscriptionVkontakteFunctionUrl.PostJsonAsync(communities)
                    .ReceiveJson<List<Tweet>>();

                return vkontaktePosts.Cast<ISocialPost>().ToList();
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }

            return new List<ISocialPost>();
        }
    }
}
