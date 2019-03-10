namespace DotNetRu.Clients.Portable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.UI;
    using DotNetRu.Utils;
    using Flurl.Http;

    public class TweetService
    {
        public static async Task<List<Tweet>> Get()
        {
            try
            {
                var config = AppConfig.GetConfig();

                var tweets = await config.TweetFunctionUrl
                    .GetJsonAsync<List<Tweet>>();

                return tweets;
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }

            return new List<Tweet>();
        }
    }
}
