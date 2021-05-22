using System;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;

namespace DotNetRu.DataStore.Audit.Extensions
{
    public static class CommunityExtensions
    {
        public static CommunityModel ToModel(this Community community)
        {
            var result = new CommunityModel
            {
                Id = community?.Id,
                City = community?.City,
                Name = community?.Name
            };

            if (!string.IsNullOrWhiteSpace(community?.VkUrl))
                result.VkUrl = new Uri(community.VkUrl);
            if (!string.IsNullOrWhiteSpace(community?.TwitterUrl))
                result.TwitterUrl = new Uri(community.TwitterUrl);
            if (!string.IsNullOrWhiteSpace(community?.TelegramChannelUrl))
                result.TelegramChannelUrl = new Uri(community.TelegramChannelUrl);
            if (!string.IsNullOrWhiteSpace(community?.TelegramChatUrl))
                result.TelegramChatUrl = new Uri(community.TelegramChatUrl);
            if (!string.IsNullOrWhiteSpace(community?.TimePadUrl))
                result.TimePadUrl = new Uri(community.TimePadUrl);
            if (!string.IsNullOrWhiteSpace(community?.MeetupComUrl))
                result.MeetupComUrl = new Uri(community.MeetupComUrl);
            if (!string.IsNullOrWhiteSpace(community?.LogoUrl))
                result.LogoUrl = new Uri(community.LogoUrl);

            return result;
        }
    }
}
