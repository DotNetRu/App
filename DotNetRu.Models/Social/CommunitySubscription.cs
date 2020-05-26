namespace DotNetRu.Models.Social
{
    public class CommunitySubscription
    {
        public SocialMediaType Type { get; set; }

        public bool IsSelected { get; set; }

        public string CommunityName { get; set; }

        public byte LoadedPosts { get; set; }
    }
}
