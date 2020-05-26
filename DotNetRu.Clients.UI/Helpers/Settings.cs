using System.Collections.Generic;
using System.Linq;
using DotNetRu.Clients.UI;
using DotNetRu.Models.Social;
using Newtonsoft.Json;

namespace DotNetRu.Clients.Portable.Helpers
{
    using System;

    using DotNetRu.Clients.Portable.ViewModel;
    using Xamarin.Essentials;

    public static class Settings
    {
        public static Language? CurrentLanguage
        {
            get
            {
                string languageCode = Preferences.Get(nameof(CurrentLanguage), null);
                return languageCode == null ? (Language?)null : GetLanguage(languageCode);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                string languageCode = value.Value.GetLanguageCode();
                Preferences.Set(nameof(CurrentLanguage), languageCode);
            }
        }

        private static Language GetLanguage(string languageCode)
        {
            if (languageCode == "ru")
            {
                return Language.Russian;
            }

            return Language.English;
        }

        private static IList<CommunitySubscription> _defaultCommunitySubscriptions;
        internal static IList<CommunitySubscription> DefaultCommunitySubscriptions
        {
            get
            {
                return _defaultCommunitySubscriptions ??= AppConfig.GetConfig()
                    .CommunityGroups.ToList();
            }
        }

        public static IList<CommunitySubscription> CommunitySubscriptions
        {
            get
            {
                string communitySubscriptions = Preferences.Get(nameof(CommunitySubscriptions), JsonConvert.SerializeObject(DefaultCommunitySubscriptions));
                return JsonConvert.DeserializeObject<IList<CommunitySubscription>>(communitySubscriptions);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                string communitySubscriptions = JsonConvert.SerializeObject(value);
                Preferences.Set(nameof(CommunitySubscriptions), communitySubscriptions);
            }
        }
    }
}
