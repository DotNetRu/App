using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetRu.AzureService;
using DotNetRu.Models.Social;
using LinqToTwitter;
using Microsoft.Extensions.Logging;

namespace DotNetRu.Azure
{
    internal static class TweetService
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger(nameof(TweetService));

        /// <summary>
        /// Returns tweets by SpdDotNet/DotNetRu (if it's retweet then original tweet is returned instead of retweet)
        /// </summary>
        /// <returns>
        /// Returns a list of tweets.
        /// </returns>
        internal static async Task<List<ISocialPost>> GetAsync(TweetSettings tweetSettings)
        {
            try
            {
                var auth = new ApplicationOnlyAuthorizer
                {
                    CredentialStore =
                                       new InMemoryCredentialStore
                                       {
                                           ConsumerKey =
                                               tweetSettings.ConsumerKey,
                                           ConsumerSecret =
                                               tweetSettings.ConsumerSecret
                                       },
                };
                await auth.AuthorizeAsync();

                using var twitterContext = new TwitterContext(auth);
                var spbDotNetTweets =
                    await (from tweet in twitterContext.Status
                        where tweet.Type == StatusType.User && tweet.ScreenName == "spbdotnet"
                                                            && tweet.TweetMode == TweetMode.Extended
                        select tweet).ToListAsync();

                var dotnetRuTweets =
                    await (from tweet in twitterContext.Status
                        where tweet.Type == StatusType.User && tweet.ScreenName == "DotNetRu"
                                                            && tweet.TweetMode == TweetMode.Extended
                        select tweet).ToListAsync();

                var unitedTweets = spbDotNetTweets.Union(dotnetRuTweets).Where(tweet => !tweet.PossiblySensitive).Select(GetTweet);

                var tweetsWithoutDuplicates = unitedTweets.GroupBy(tw => tw.StatusId).Select(g => g.First());

                var sortedTweets = tweetsWithoutDuplicates.OrderByDescending(x => x.CreatedDate).Cast<ISocialPost>().ToList();

                return sortedTweets;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Unhandled error while getting original tweets");
            }

            return new List<ISocialPost>();
        }

        private static Tweet GetTweet(Status tweet)
        {
            var sourceTweet = tweet.RetweetedStatus.StatusID == 0 ? tweet : tweet.RetweetedStatus;

            var urlLinks =
                sourceTweet.Entities.UrlEntities.Select(t => new KeyValuePair<string, string>(t.Url, t.DisplayUrl)).ToList();

            var profileImage = sourceTweet.User?.ProfileImageUrl.Replace("http://", "https://", StringComparison.InvariantCultureIgnoreCase);
            if (profileImage != null)
            {
                //normal image is 48x48, bigger image is 73x73, see https://developer.twitter.com/en/docs/accounts-and-users/user-profile-images-and-banners
                profileImage = Regex.Replace(profileImage, @"(.+)_normal(\..+)", "$1_bigger$2");
            }

            return new Tweet(sourceTweet.StatusID)
            {
                PostedImage =
                               tweet.Entities?.MediaEntities.Count > 0
                                   ? tweet.Entities?.MediaEntities?[0].MediaUrlHttps ?? string.Empty
                                   : string.Empty,
                NumberOfLikes = sourceTweet.FavoriteCount,
                NumberOfReposts = sourceTweet.RetweetCount,
                ScreenName = sourceTweet.User?.ScreenNameResponse ?? string.Empty,
                Text = sourceTweet.FullText.ConvertToUsualUrl(urlLinks),
                Name = sourceTweet.User?.Name,
                CreatedDate = tweet.CreatedAt,
                Url = $"https://twitter.com/{sourceTweet.User?.ScreenNameResponse}/status/{tweet.StatusID}",
                Image = profileImage
            };
        }
    }
}
