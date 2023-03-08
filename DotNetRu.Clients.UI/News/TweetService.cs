using System.Linq;
using DotNetRu.Models.Social;

namespace DotNetRu.Clients.Portable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DotNetRu.AppUtils.Config;
    using DotNetRu.AppUtils.Logging;
    using Flurl.Http;

    public static class TweetService
    {
        public static async Task<List<ISocialPost>> GetAsync()
        {
            try
            {
                var config = AppConfig.GetConfig();

                var tweets = await config.TweetFunctionUrl
                    .GetJsonAsync<List<Tweet>>();

                return tweets.Cast<ISocialPost>().ToList();
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }

            return new List<ISocialPost>();
        }

        public static async Task<List<ISocialPost>> GetBySubscriptionsAsync(List<string> communities)
        {
            try
            {
                var config = AppConfig.GetConfig();

                var tweets = await config.SubscriptionTweetFunctionUrl.PostJsonAsync(communities)
                    .ReceiveJson<List<Tweet>>();

                return tweets.Cast<ISocialPost>().ToList();
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }

            return new List<ISocialPost>();
        }
    }
}
