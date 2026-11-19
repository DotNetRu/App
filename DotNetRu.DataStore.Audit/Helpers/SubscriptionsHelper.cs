using System;
using System.Collections.Generic;
using System.Linq;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.Services;
using DotNetRu.Models.Social;
using Realms;

namespace DotNetRu.DataStore.Audit.Helpers
{
    public class SubscriptionsHelper
    {
        public static IList<SubscriptionModel> GetDefaultCommunitySubscriptions()
        {
            return DefaultCommunitySubscriptions.Union(RealmService.Get<CommunityModel>().Select(x => new SubscriptionModel
            {
                Community = x,
                IsSelected = true,
                LoadedPosts = 10,
                Type = SocialMediaType.Vkontakte
            })).ToList();
        }

        public static IList<SubscriptionModel> GetDefaultCommunitySubscriptionsByRealm(Realm realm)
        {
            return DefaultCommunitySubscriptions.Union(RealmService.Get<CommunityModel>(realm).Select(x => new SubscriptionModel
            {
                Community = x,
                IsSelected = true,
                LoadedPosts = 100,
                Type = SocialMediaType.Vkontakte
            })).ToList();
        }

        public static IList<SubscriptionModel> DefaultCommunitySubscriptions => new List<SubscriptionModel>
        {
            new SubscriptionModel
            {
                IsSelected = true,
                LoadedPosts = 100,
                Community = new CommunityModel
                {
                    Name = "DotNetRu",
                    City = "Общие",
                    LogoUrl = new Uri(
                        "https://raw.githubusercontent.com/AnatolyKulakov/SpbDotNet/master/Swag/dotnetru-squared-logo-bordered/dotnetru-squared-logo-br-200.png"),
                    VkUrl = new Uri("https://vk.com/DotNetRu"),
                    TelegramChannelUrl = new Uri("https://t.me/DotNetRu"),
                    TelegramChatUrl = new Uri("https://t.me/DotNetRuChat")
                },
                Type = SocialMediaType.Vkontakte
            }
        };
    }
}
