using DotNetRu.Clients.Portable.Model;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Models.Social;

namespace DotNetRu.Clients.UI.About
{
    public class AboutPageItem
    {
        public AboutItemType AboutItemType { get; set; }

        public CommunityModel Community { get; set; }

        public MenuItem MenuItem { get; set; }

        public string Text { get; set; }

        public CommunitySubscription Subscription { get; set; }
    }
}
