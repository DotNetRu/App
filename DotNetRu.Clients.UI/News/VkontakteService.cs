namespace DotNetRu.Clients.Portable.Services
{

using System.Linq;
using DotNetRu.Models.Social;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetRu.AppUtils.Config;
using DotNetRu.AppUtils.Logging;
using Flurl.Http;

public static class VkontakteService
{
    public static async Task<IList<ISocialPost>> GetVkPostsAsync(IDictionary<string, byte> communities)
    {
        try
        {
            var config = AppConfig.GetConfig();

            var vkontaktePosts = await config.VkontakteFunctionUrl
                .GetJsonAsync<List<VkontaktePost>>();

            return vkontaktePosts.Cast<ISocialPost>().ToList();
        }
        catch (Exception e)
        {
            DotNetRuLogger.Report(e);
        }

        return new List<ISocialPost>();
    }
}
}
