namespace DotNetRu.Clients.Portable.Services;

using System.Linq;
using DotNetRu.Models.Social;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetRu.AppUtils.Config;
using DotNetRu.AppUtils.Logging;
using Flurl;
using Flurl.Http;

public static class TweetService
{
    public static async Task<IList<ISocialPost>> GetTweetsAsync(IList<string> communities)
    {
        try
        {
            var config = AppConfig.GetConfig();

            var tweets = await config.TweetFunctionUrl
                .SetQueryParam("communities", communities)
                .GetJsonAsync<IList<Tweet>>();

            return tweets.Cast<ISocialPost>().ToList();
        }
        catch (Exception e)
        {
            DotNetRuLogger.Report(e);
        }

        return new List<ISocialPost>();
    }
}
