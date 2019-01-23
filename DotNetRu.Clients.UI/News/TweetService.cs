namespace DotNetRu.Clients.Portable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Utils;
    using Flurl;
    using Flurl.Http;

    public class TweetService
    {
        public static async Task<List<Tweet>> Get()
        {
            try
            {
                var tweets = await "https://tweetazure.azurewebsites.net/api"
                    .AppendPathSegment("tweets")
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
