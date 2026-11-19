using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AppUtils.Config;
using DotNetRu.Models.Social;
using Flurl.Http;

namespace DotNetRu.Clients.Portable.Services
{
    public static class VkontakteService
    {
        public static async Task<IEnumerable<VkontaktePost>> GetVkPostsAsync(IDictionary<string, byte> communities)
        {
            if (communities.Count == 0)
            {
                return Array.Empty<VkontaktePost>();
            }

            var posts = await AppConfig.GetConfig().VkontakteFunctionUrl.GetJsonAsync<List<VkontaktePost>>();
            return posts.Where(post => post.CommunityGroupId != null && communities.ContainsKey(post.CommunityGroupId))
                .GroupBy(post => post.CommunityGroupId, StringComparer.OrdinalIgnoreCase)
                .SelectMany(group => group.OrderByDescending(post => post.CreatedDate).Take(communities[group.Key]));
        }
    }
}
