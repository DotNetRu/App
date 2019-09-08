using System;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;

namespace DotNetRu.DataStore.Audit.Extensions
{
    public static class CommunityExtensions
    {
        public static CommunityModel ToModel(this Community community)
        {
            return new CommunityModel
            {
                Id = community.Id,
                City = community.City,
                Name = community.Name,
                VkUrl = new Uri(community.VkUrl),
                LogoUrl = new Uri(community.LogoUrl)
            };
        }
    }
}
