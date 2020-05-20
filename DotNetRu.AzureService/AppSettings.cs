using System.Collections.Generic;

namespace DotNetRu.AzureService
{
    public class RealmSettings
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string RealmServerUrl { get; set; }

        public string RealmName { get; set; }
    }

    public class TweetSettings
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public List<string> CommunityGroups { get; set; }
    }

    public class VkontakteSettings
    {
        public string ServiceKey { get; set; }

        public Dictionary<string, ulong> CommunityGroups { get; set; }
    }
}
