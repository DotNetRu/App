using DotNetRu.Clients.Portable.Model;
using DotNetRu.DataStore.Audit.Models;

namespace DotNetRu.Clients.UI.About
{
    public class AboutPageItem
    {
        public AboutItemType AboutItemType { get; set; }

        public CommunityModel Community { get; set; }

        public Portable.Model.MenuItem MenuItem { get; set; }

        public string Text { get; set; }
    }
}
