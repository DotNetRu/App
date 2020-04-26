using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRu.AzureService;
using DotNetRu.Models.Social;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace DotNetRu.Azure
{
    internal static class VkontakteService
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger(nameof(VkontakteService));

        private static List<Group> _communityGroups = new List<Group>();

        /// <summary>
        /// Returns vkontakte posts by community groups
        /// </summary>
        /// <returns>
        /// Returns a list of vkontakte posts.
        /// </returns>
        internal static async Task<List<ISocialPost>> GetAsync(VkontakteSettings vkontakteSettings)
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
                    {
                        return Math.Abs(groupId).ToString();
                    }

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
                    result.AddRange(communityGroupPosts.Select(communityGroupPost => GetVkontaktePost(communityGroupPost, communityGroup.Key)));
                }

                var postsWithoutDuplicates = result.GroupBy(p => p.PostId).Select(g => g.First()).ToList();

                // remove reposted duplicates
                var postIds = postsWithoutDuplicates.Select(x => x.PostId).ToList();
                for (int i = 0; i < postsWithoutDuplicates.Count; i++)
                {
                    if (postsWithoutDuplicates[i].CopyHistory.Select(x => x.PostId).Intersect(postIds).Any())
                    {
                        postsWithoutDuplicates.RemoveAt(i);
                    }
                }

                // set correct name for posts from user
                var users = await api.Users.GetAsync(postsWithoutDuplicates
                    .Where(x => x.OwnerId != null && x.FromId != null && x.OwnerId != x.FromId)
                    .Select(x => x.FromId.ToString()));
                foreach (var post in postsWithoutDuplicates.Where(x => x.OwnerId != null && x.FromId != null && x.OwnerId != x.FromId))
                {
                    var user = users.FirstOrDefault(x => x.Id == post.FromId);
                    if (user?.LastName != null)
                    {
                        post.Name = $"{user.FirstName} {user.LastName}";
                    }
                }

                var sortedPosts = postsWithoutDuplicates.OrderByDescending(x => x.CreatedDate).Cast<ISocialPost>().ToList();

                return sortedPosts;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Unhandled error while getting original vkontakte posts");
            }

            return new List<ISocialPost>();
        }

        private static VkontaktePost GetVkontaktePost(Post post, string communityGroupId)
        {
            var currentGroup = post.OwnerId != null ? _communityGroups.FirstOrDefault(x => x.Id == Math.Abs((long)post.OwnerId)) : null;
            return new VkontaktePost(post.Id)
            {
                //ToDo: разобраться, откуда правильно брать PostedImage
                //PostedImage = $"https://vk.com/{(long.TryParse(communityGroupId, out var groupId) ? $"club{Math.Abs(groupId)}" : communityGroupId)}?w=wall{post.OwnerId}_{post.Id}",
                PostedImage = post.Attachments?.Where(x => x.Type == typeof(Link) && (x.Instance as Link)?.Image != null).ToList().Count > 0
                    ? (post.Attachments?.Where(x => x.Type == typeof(Link) && (x.Instance as Link)?.Image != null).ToList()[0].Instance as Link)?.Image ?? string.Empty
                    : string.Empty,
                NumberOfViews = post.Views?.Count,
                NumberOfLikes = post.Likes?.Count,
                NumberOfReposts = post.Reposts?.Count,
                FromId = post.FromId,
                OwnerId = post.OwnerId,
                ScreenName = currentGroup?.ScreenName,
                Text = post.Text,
                //ToDo: разобраться, что выводить, если Text пуст
                //Text = string.IsNullOrWhiteSpace(post.Text)
                //    ? (post.Attachments?.FirstOrDefault(x => x.Type == typeof(Link) && x.Instance is Link)?.Instance as Link)?.Description
                //    : post.Text,
                Name = currentGroup?.Name,
                CreatedDate = post.Date?.ToLocalTime(),
                Url = $"https://vk.com/{(long.TryParse(communityGroupId, out var groupId) ? $"club{Math.Abs(groupId)}" : communityGroupId)}?w=wall{post.OwnerId}_{post.Id}",
                //ToDo: сейчас отображается логотип сообщества, а не изображение из поста. Исправить
                Image = currentGroup?.Photo200?.ToString(),
                CopyHistory = post.CopyHistory.Select(x => new CopyHistory
                {
                    PostId = x.Id,
                    FromId = x.FromId
                }).ToList(),
            };
        }
    }
}
