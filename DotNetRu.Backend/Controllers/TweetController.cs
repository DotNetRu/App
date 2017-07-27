using Censored;
using LinqToTwitter;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using XamarinEvolve.Backend.Models;
using System.Configuration;
using XamarinEvolve.Backend.Helpers;

namespace XamarinEvolve.Backend.Controllers
{
    [MobileAppController]
    public class TweetController : ApiController
    {
        public class MemoryCacher
        {
            public object GetValue(string key)
            {
                MemoryCache memoryCache = MemoryCache.Default;
                return memoryCache.Get(key);
            }

            public bool Add(string key, object value, DateTimeOffset absExpiration)
            {
                MemoryCache memoryCache = MemoryCache.Default;
                return memoryCache.Add(key, value, absExpiration);
            }

            public void Delete(string key)
            {
                MemoryCache memoryCache = MemoryCache.Default;
                if (memoryCache.Contains(key))
                {
                    memoryCache.Remove(key);
                }
            }
        }

        readonly MemoryCacher cache;
        const string words = "";
        public TweetController()
        {
            censor = new Censor(words.Split(new[] { ',' }));
            cache = new MemoryCacher();
            
        }
        readonly Censor censor;
        public async Task<string> Get()
        {            
            try
            {               
                var tweets = new List<Tweet>();
                var tweetsObj = cache.GetValue("tweets");

                if(tweetsObj != null)
                    tweets = tweetsObj as List<Tweet>;

                if (tweets == null)
                    tweets = new List<Tweet>();

                if (tweets.Count > 0)
                    return Newtonsoft.Json.JsonConvert.SerializeObject(tweets);

                var auth = new ApplicationOnlyAuthorizer()
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"],
                        ConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                    },
                };
                await auth.AuthorizeAsync();

                var twitterContext = new TwitterContext(auth);

                // Find tweets by user
                //var user =
                //   await
                //   (from u in twitterContext.User
                //    where u.Type == UserType.Lookup
                //    && u.ScreenNameList == FeatureFlags.TwitterHandle.Replace("@", "")
                //    select u).ToListAsync();

                //if (user != null)
                //{
                //    tweets.AddRange(from s in user
                //                    where s.Status != null
                //                    select new Tweet
                //                    {
                //                        TweetedImage = s.Status.Entities?.MediaEntities.Count > 0 ? s.Status.Entities?.MediaEntities?[0].MediaUrlHttps ?? string.Empty : string.Empty,
                //                        ScreenName = s.ScreenNameResponse ?? string.Empty,
                //                        Text = s.Status.Text,
                //                        Name = s.Status.User.Name,
                //                        CreatedDate = s.Status.CreatedAt,
                //                        Url = string.Format("https://twitter.com/{0}/status/{1}", s.Status.User.ScreenNameResponse, s.Status.StatusID),
                //                        Image = (s.Status.RetweetedStatus != null && s.Status.RetweetedStatus.User != null ?
                //                         s.Status.RetweetedStatus.User.ProfileImageUrl.Replace("http://", "https://") : s.Status.User.ProfileImageUrl.Replace("http://", "https://"))
                //                    });
                //}

                var queryResponse = await
                    (from tweet in twitterContext.Search
                     where tweet.Type == SearchType.Search &&
                         (tweet.Query == $"{FeatureFlags.TwitterHashtag} OR {FeatureFlags.TwitterHandle}") &&
                         tweet.Count == 100
                     select tweet).SingleOrDefaultAsync();

                if (queryResponse?.Statuses != null)
                {
                    tweets.AddRange(
                        (from tweet in queryResponse.Statuses
                         where tweet.RetweetedStatus.StatusID == 0 && !tweet.PossiblySensitive && !censor.HasCensoredWord(tweet.Text)
                         select new Tweet
                         {
                             TweetedImage = tweet.Entities?.MediaEntities.Count > 0 ? tweet.Entities?.MediaEntities?[0].MediaUrlHttps ?? string.Empty : string.Empty,
                             ScreenName = tweet.User?.ScreenNameResponse ?? string.Empty,
                             Text = tweet.Text,
                             Name = tweet.User.Name,
                             CreatedDate = tweet.CreatedAt,
                             Url = string.Format("https://twitter.com/{0}/status/{1}", tweet.User.ScreenNameResponse, tweet.StatusID),
                             Image = (tweet.RetweetedStatus != null && tweet.RetweetedStatus.User != null ?
                                     tweet.RetweetedStatus.User.ProfileImageUrl.Replace("http://", "https://") : tweet.User.ProfileImageUrl.Replace("http://", "https://"))
                         }).Take(15).ToList());
                }

                tweets = tweets.OrderByDescending(t => t.CreatedDate).ToList();

                cache.Add("tweets", tweets, DateTimeOffset.UtcNow.AddMinutes(1));
               
                return Newtonsoft.Json.JsonConvert.SerializeObject(tweets);
            }
            catch (Exception e)
            {
                //return e.ToString();
            }


            return string.Empty;
        }
    }
}
