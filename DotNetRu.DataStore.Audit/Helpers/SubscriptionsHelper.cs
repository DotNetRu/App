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

        private static IList<SubscriptionModel> _defaultCommunitySubscriptions;

        public static IList<SubscriptionModel> DefaultCommunitySubscriptions
        {
            get
            {
                return _defaultCommunitySubscriptions ??= new List<SubscriptionModel>
                {
                    //ToDo: получать из настроек (для теста пока так)
                    new SubscriptionModel
                    {
                        IsSelected = true,
                        LoadedPosts = 0,
                        Community = new CommunityModel
                        {
                            Name = "DotNetRu",
                            City = "Общие",
                            LogoUrl = new Uri(
                                "https://raw.githubusercontent.com/AnatolyKulakov/SpbDotNet/master/Swag/dotnetru-squared-logo-bordered/dotnetru-squared-logo-br-200.png")
                        },
                        Type = SocialMediaType.Twitter
                    },
                    new SubscriptionModel
                    {
                        IsSelected = true,
                        LoadedPosts = 0,
                        Community = new CommunityModel
                        {
                            Name = "SpbDotNet",
                            City = "Санкт-Петербург",
                            LogoUrl = new Uri(
                                "https://raw.githubusercontent.com/AnatolyKulakov/SpbDotNet/master/Swag/spbdotnet-squared-logo-bordered/spbdotnet-squared-logo-br-200.png")
                        },
                        Type = SocialMediaType.Twitter
                    },
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
                            VkUrl = new Uri("https://vk.com/DotNetRu")
                        },
                        Type = SocialMediaType.Vkontakte
                    }
                };
            }
        }
    }
}
