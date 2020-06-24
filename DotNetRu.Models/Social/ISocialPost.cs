using System;

namespace DotNetRu.Models.Social
{
    public interface ISocialPost
    {
        public SocialMediaType SocialMediaType { get; }

        public bool HasImage { get; }

        public string PostedImage { get; set; }

        public bool HasVideo { get; }

        public PostedVideo PostedVideo { get; set; }

        public int? NumberOfViews { get; set; }

        public int? NumberOfLikes { get; set; }

        public int? NumberOfReposts { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string ScreenName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string TitleDisplay { get; }

        public string SubtitleDisplay { get; }

        public string DateDisplay { get; }

        public Uri PostedImageUri { get; }

        public bool HasAttachedImage { get; }

        public Uri PostedVideoUri { get; }

        public bool HasAttachedVideo { get; }

        public Uri PostedVideoImageUri { get; }
    }
}
