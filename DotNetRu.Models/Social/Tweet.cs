using System;
using System.Runtime.Serialization;

namespace DotNetRu.Models.Social
{
    public class Tweet : ISocialPost
    {
        public Tweet(ulong statusId)
        {
            StatusId = statusId;
        }

        [IgnoreDataMember]
        public ulong StatusId { get; }

        public SocialMediaType SocialMediaType => SocialMediaType.Twitter;

        private string postedImage;

        public bool HasImage => !string.IsNullOrWhiteSpace(this.postedImage);

        public string PostedImage
        {
            get => this.postedImage;
            set => this.postedImage = value;
        }

        private PostedVideo postedVideo;

        public bool HasVideo => this.postedVideo != null;

        public PostedVideo PostedVideo
        {
            get => this.postedVideo;
            set => this.postedVideo = value;
        }

        public int? NumberOfViews { get; set; }

        public int? NumberOfLikes { get; set; }

        public int? NumberOfReposts { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string ScreenName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string TitleDisplay => this.Name;

        public string SubtitleDisplay => "@" + this.ScreenName;

        public string DateDisplay => this.CreatedDate?.ToShortDateString();

        public Uri PostedImageUri
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this.PostedImage))
                    {
                        return null;
                    }

                    return new Uri(this.PostedImage);
                }
                catch
                {
                    // TODO ignored
                }

                return null;
            }
        }

        public bool HasAttachedImage => !string.IsNullOrWhiteSpace(this.PostedImage);

        public Uri PostedVideoUri
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this.PostedVideo?.Uri))
                    {
                        return null;
                    }

                    return new Uri(this.PostedVideo.Uri);
                }
                catch
                {
                    // TODO ignored
                }

                return null;
            }
        }

        public Uri PostedVideoImageUri => this.PostedVideo?.ImageUri;

        public bool HasAttachedVideo => this.PostedVideo != null;

        public override string ToString()
        {
            return $"[Name={Name};Text={Text};Reposts={NumberOfReposts};Likes={NumberOfLikes}";
        }
    }
}
