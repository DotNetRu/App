using System;

namespace DotNetRu.DataStore.Audit.Models
{
    public class CommunityModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public Uri VkUrl { get; set; }

        public Uri TwitterUrl { get; set; }

        public Uri TelegramChannelUrl { get; set; }

        public Uri TelegramChatUrl { get; set; }

        public Uri TimePadUrl { get; set; }

        public Uri MeetupComUrl { get; set; }

        public Uri LogoUrl { get; set; }
    }
}
