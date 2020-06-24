using DotNetRu.Models.Social;

namespace DotNetRu.DataStore.Audit.Models
{
    public class SubscriptionModel
    {
        public SocialMediaType Type { get; set; }

        public bool IsSelected { get; set; }

        public byte LoadedPosts { get; set; }

        public CommunityModel Community { get; set; }
    }
}
