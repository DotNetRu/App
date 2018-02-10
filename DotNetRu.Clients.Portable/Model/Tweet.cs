namespace DotNetRu.Clients.Portable.Model
{
    using System;
    using System.Globalization;

    using Humanizer;

    using Newtonsoft.Json;

    public class Tweet
    {
        private string tweetedImage;

        [JsonIgnore]
        public bool HasImage => !string.IsNullOrWhiteSpace(this.tweetedImage);

        [JsonProperty("tweetedImage")]
        public string TweetedImage
        {
            get => this.tweetedImage;
            set => this.tweetedImage = value;
        }

        [JsonProperty("likes")]
        public int? NumberOfLikes { get; set; }

        [JsonProperty("retweets")]
        public int NumberOfRetweets { get; set; }

        [JsonProperty("comments")]
        public int NumberOfComments { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screenName")]
        public string ScreenName { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public string TitleDisplay => this.Name;

        [JsonIgnore]
        public string SubtitleDisplay => "@" + this.ScreenName;

        [JsonIgnore]
        public string DateDisplay => this.CreatedDate.Humanize(culture: CultureInfo.InvariantCulture);

        [JsonIgnore]
        public Uri TweetedImageUri
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this.TweetedImage))
                    {
                        return null;
                    }

                    return new Uri(this.TweetedImage);
                }
                catch
                {
                    // TODO ignored
                }

                return null;
            }
        }

        public bool HasAttachedImage => !string.IsNullOrWhiteSpace(this.TweetedImage);
    }
}