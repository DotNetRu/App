using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AzureService;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace DotNetRu.Azure
{
    internal class VkontakteService
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger(nameof(VkontakteService));

        private static List<Group> _communityGroups = new List<Group>();

        /// <summary>
        /// Returns vkontakte posts by community groups
        /// </summary>
        /// <returns>
        /// Returns a list of vkontakte posts.
        /// </returns>
        internal static async Task<List<VkontaktePost>> GetAsync(VkontakteSettings vkontakteSettings)
        {
            var result = new List<VkontaktePost>();
            try
            {
                using var api = new VkApi();
                api.Authorize(new ApiAuthParams
                {
                    AccessToken = vkontakteSettings.ServiceKey
                });

                _communityGroups = (await api.Groups.GetByIdAsync(vkontakteSettings.CommunityGroups.Select(x =>
                {
                    if (long.TryParse(x.Key, out var groupId))
                        return Math.Abs(groupId).ToString();

                    return x.Key;
                }), null, GroupsFields.All)).ToList();
                foreach (var communityGroup in vkontakteSettings.CommunityGroups)
                {
                    var wallGetParams = new WallGetParams { Count = communityGroup.Value };
                    if (long.TryParse(communityGroup.Key, out var groupId))
                    {
                        wallGetParams.OwnerId = groupId;
                    }
                    else
                    {
                        wallGetParams.Domain = communityGroup.Key;
                    }
                    var communityGroupPosts = (await api.Wall.GetAsync(wallGetParams)).WallPosts;
                    foreach (var communityGroupPost in communityGroupPosts)
                    {
                        result.Add(GetVkontaktePost(communityGroupPost, communityGroup.Key));
                    }
                }

                //ToDo: удалить дубликаты
                //ToDo: обрабатывать репосты
                var postsWithoutDuplicates = result.GroupBy(p => p.PostId).Select(g => g.First());

                var sortedPosts = postsWithoutDuplicates.OrderByDescending(x => x.CreatedDate).ToList();

                return sortedPosts;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Unhandled error while getting original vkontakte posts");
            }

            return new List<VkontaktePost>();
        }

        private static VkontaktePost GetVkontaktePost(Post post, string communityGroupId)
        {
            var currentGroup = post.OwnerId != null ? _communityGroups.FirstOrDefault(x => x.Id == Math.Abs((long)post.OwnerId)) : null;
            //var postedImagePhoto = (post.Attachments.FirstOrDefault(x => x.Type == typeof(Link))?.Instance as Link)?.Photo;
            //if (postedImagePhoto != null)
            //{
            //    new VkApi().Photo.GetById(new List<string> { $"2000022727_457296869" })
            //}

            return new VkontaktePost(post.Id)
            {
                //PostedImage = $"https://vk.com/{(long.TryParse(communityGroupId, out var groupId) ? $"club{Math.Abs(groupId)}" : communityGroupId)}?w=wall{post.OwnerId}_{post.Id}",
                NumberOfViews = post.Views?.Count,
                NumberOfLikes = post.Likes?.Count,
                NumberOfReposts = post.Reposts?.Count,
                FromId = post.FromId,
                OwnerId = post.OwnerId,
                CreatedDate = post.Date?.ToLocalTime(),
                Name = currentGroup?.Name,
                ScreenName = currentGroup?.ScreenName,
                Text = post.Text,
                Url = $"https://vk.com/{(long.TryParse(communityGroupId, out var groupId) ? $"club{Math.Abs(groupId)}" : communityGroupId)}?w=wall{post.OwnerId}_{post.Id}",
                CopyHistory = post.CopyHistory.Select(x => new CopyHistory
                {
                    PostId = x.Id,
                    FromId = x.FromId
                }).ToList(),
                Image = currentGroup?.Photo200?.ToString()
            };
        }
    }
}
