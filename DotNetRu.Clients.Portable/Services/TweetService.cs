namespace DotNetRu.Clients.Portable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Utils;
    using DotNetRu.Utils.Helpers;

    using LinqToTwitter;

    public class TweetService
    {
        public static async Task<List<Tweet>> Get()
        {
            try
            {
                var auth = new ApplicationOnlyAuthorizer
                               {
                                   CredentialStore =
                                       new InMemoryCredentialStore
                                           {
                                               ConsumerKey =
                                                   "ho0v2B1bimeufLqI1rA8KuLBp",
                                               ConsumerSecret =
                                                   "RAzIHxhkzINUxilhdr98TWTtjgFKXYzkEhaGx8WJiBPh96TXNK"
                                           },
                               };
                await auth.AuthorizeAsync();

                var twitterContext = new TwitterContext(auth);

                var spbDotNetTweets =
                    await (from tweet in twitterContext.Status
                           where tweet.Type == StatusType.User && tweet.ScreenName == "spbdotnet"
                                                               && tweet.TweetMode == TweetMode.Extended
                           select tweet).ToListAsync();

                var dotnetruTweets =
                    await (from tweet in twitterContext.Status
                           where tweet.Type == StatusType.User && tweet.ScreenName == "DotNetRu"
                                                               && tweet.TweetMode == TweetMode.Extended
                           select tweet).ToListAsync();

                var tweets = spbDotNetTweets.Union(dotnetruTweets).Where(tweet => !tweet.PossiblySensitive)
                    .Select(GetTweet).OrderByDescending(x => x.CreatedDate).ToList();

                return tweets;
            }
            catch (Exception e)
            {
                new DotNetRuLogger().Report(e);
            }

            return new List<Tweet>();
        }

        private static Tweet GetTweet(Status tweet)
        {
            var sourceTweet = tweet.RetweetedStatus.StatusID == 0 ? tweet : tweet.RetweetedStatus;

            var urlLinks =
                sourceTweet.Entities.UrlEntities.Select(t => new KeyValuePair<string, string>(t.Url, t.DisplayUrl)).ToList();

            return new Tweet
                       {
                           TweetedImage =
                               tweet.Entities?.MediaEntities.Count > 0
                                   ? tweet.Entities?.MediaEntities?[0].MediaUrlHttps ?? string.Empty
                                   : string.Empty,
                           NumberOfLikes = sourceTweet.FavoriteCount,
                           NumberOfRetweets = sourceTweet.RetweetCount,
                           ScreenName = sourceTweet.User?.ScreenNameResponse ?? string.Empty,
                           Text = sourceTweet.FullText.ConvertToUsualUrl(urlLinks),
                           Name = sourceTweet.User?.Name,
                           CreatedDate = tweet.CreatedAt,
                           Url = $"https://twitter.com/{sourceTweet.User?.ScreenNameResponse}/status/{tweet.StatusID}",
                           Image = sourceTweet.User?.ProfileImageUrl.Replace("http://", "https://")
                       };
        }
    }
}