namespace DotNetRu.Clients.Portable.Model
{
    using System;

    public class Tweet
    {
        public bool HasImage { get; set; }

        public string TweetedImage { get; set; }

        public int NumberOfLikes { get; set; }

        public int NumberOfRetweets { get; set; }

        public int NumberOfComments { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string ScreenName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string TitleDisplay { get; set; }

        public string SubtitleDisplay { get; set; }

        public string DateDisplay { get; set; }

        public string TweetedImageUri { get; set; }

        public bool HasAttachedImage { get; set; }
    }
}
