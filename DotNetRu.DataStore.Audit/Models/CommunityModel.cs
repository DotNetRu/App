using System;

namespace DotNetRu.DataStore.Audit.Models
{
    public class CommunityModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public Uri VkUrl { get; set; }

        public Uri LogoUrl { get; set; }
    }
}
