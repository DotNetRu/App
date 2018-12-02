namespace DotNetRu.Clients.Portable.Model
{
    using System;

    public class Tweet
    {
        private string tweetedImage;

        public bool HasImage => !string.IsNullOrWhiteSpace(this.tweetedImage);

        public string TweetedImage
        {
            get => this.tweetedImage;
            set => this.tweetedImage = value;
        }

        public int? NumberOfLikes { get; set; }

        public int NumberOfRetweets { get; set; }

        public int NumberOfComments { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string ScreenName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string TitleDisplay => this.Name;

        public string SubtitleDisplay => "@" + this.ScreenName;

        public string DateDisplay => this.CreatedDate.ToShortDateString();

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
