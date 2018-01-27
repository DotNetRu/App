using DotNetRu.Utils.Helpers;

namespace DotNetRu.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Utils;
    using LinqToTwitter;

    public class TweetHelper
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
                        where tweet.Type == StatusType.User && tweet.ScreenName == "spbdotnet" &&
                              tweet.TweetMode == TweetMode.Extended
                        select tweet).ToListAsync();

                var dotnetruTweets =
                    await (from tweet in twitterContext.Status
                        where tweet.Type == StatusType.User && tweet.ScreenName == "DotNetRu" &&
                              tweet.TweetMode == TweetMode.Extended
                        select tweet).ToListAsync();

                var tweets =
                (from tweet in spbDotNetTweets.Union(dotnetruTweets)
                    where !tweet.PossiblySensitive
                    let tweetUser = tweet.User
                    where tweetUser != null
                    select new Tweet
                    {
                        TweetedImage =
                            tweet.Entities?.MediaEntities.Count > 0
                                ? tweet.Entities?.MediaEntities?[0].MediaUrlHttps ?? string.Empty
                                : string.Empty,
                        NumberOfLikes = tweet.FavoriteCount,
                        NumberOfRetweets = tweet.RetweetCount,
                        ScreenName = tweetUser?.ScreenNameResponse ?? string.Empty,
                        Text = String.IsNullOrEmpty(tweet.RetweetedStatus.FullText)
                            ? tweet.FullText.ConvertToUsualUrl(
                                tweet.Entities.UrlEntities.ToDictionary(t => t.Url, t => t.DisplayUrl))
                            : tweet.RetweetedStatus.FullText.ConvertToUsualUrl(
                                tweet.RetweetedStatus.Entities.UrlEntities.ToDictionary(t => t.Url, t => t.DisplayUrl)),
                        Name = tweetUser.Name,
                        CreatedDate = tweet.CreatedAt,
                        Url =
                            $"https://twitter.com/{tweetUser.ScreenNameResponse}/status/{tweet.StatusID}",
                        Image = tweet.RetweetedStatus?.User != null
                            ? tweet.RetweetedStatus.User.ProfileImageUrl.Replace(
                                "http://",
                                "https://")
                            : tweetUser.ProfileImageUrl.Replace("http://", "https://")
                    }).OrderByDescending(x => x.CreatedDate).Take(15).ToList();

                return tweets;
            }
            catch (Exception e)
            {
                new DotNetRuLogger().Report(e);
            }

            return new List<Tweet>();
        }
    }
}