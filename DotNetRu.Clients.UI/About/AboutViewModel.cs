using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Services;
using DotNetRu.Clients.UI.About;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Localization;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.Clients.UI.Pages.Info;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.Services;
using DotNetRu.Models.Social;
using DotNetRu.Utils.Helpers;
using Microsoft.AppCenter.Analytics;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DotNetRu.Clients.Portable.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {

        private Language selectedLanguage;

        public ObservableRangeCollection<Grouping<string, AboutPageItem>> AboutPageItems { get; } = new ObservableRangeCollection<Grouping<string, AboutPageItem>>();

        public AboutViewModel(INavigation navigation) : base(navigation)
        {
            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.HandleLanguageChange());

            this.selectedLanguage = LanguageService.GetCurrentLanguage();

            AboutPageItems.ReplaceRange(GetAboutItems());
        }

        private List<Grouping<string, AboutPageItem>> GetAboutItems()
        {
            var aboutPageItems = new List<Grouping<string, AboutPageItem>>();

            aboutPageItems.Add(new Grouping<string, AboutPageItem>("Friends", new [] {
                new AboutPageItem
                {
                    AboutItemType = AboutItemType.Friends
                }
            }));

            var communities = RealmService.Get<CommunityModel>();
            var sessions = RealmService.Get<MeetupModel>();
            var communitiesWithFirstSessionDate = communities.Join(
                sessions,
                c => c.Id,
                s => s.CommunityID,
                (c, s) => new
                {
                    Community = c,
                    FirstSessionDate = s.Sessions.OrderBy(session => session.StartTime).FirstOrDefault()?.StartTime
                }).OrderBy(x => x.FirstSessionDate).GroupBy(x => x.Community.Id).Select(x => x.First());
            aboutPageItems.Add(new Grouping<string, AboutPageItem>("OurCommunities", communitiesWithFirstSessionDate.Select(x => new AboutPageItem
            {
                Community = x.Community,
                AboutItemType = AboutItemType.Community
            })));

            //ToDo: добавить сортировку элементов
            var communitySubscriptions = Helpers.Settings.CommunitySubscriptions;
            aboutPageItems.Add(new Grouping<string, AboutPageItem>("Subscriptions",
                new []
                {
                    new AboutPageItem
                    {
                        AboutItemType = AboutItemType.Text,
                        Text = AppResources.SubscriptionsLabel
                    }
                }.Union(
                communitySubscriptions.Select(x => new AboutPageItem
                    {
                        Subscription = x,
                        AboutItemType = AboutItemType.Subscriptions
                    }).Union(new []
                    {
                        new AboutPageItem
                        {
                            AboutItemType = AboutItemType.MenuItem,
                            MenuItem = new Model.MenuItem { Name = "SaveSubscriptions", Parameter="SaveSubscriptions"}
                        }
                    }))));

            var aboutAppItems = new []
            {
                new Model.MenuItem { Name = "CreatedBy", Parameter="CreatedBy"},
                new Model.MenuItem { Name = "IssueTracker", Parameter="IssueTracker"},
                new Model.MenuItem { Name = "TechnologyUsed", Parameter="TechnologyUsed"},
            };
            aboutPageItems.Add(new Grouping<string, AboutPageItem>("AboutApp", aboutAppItems.Select(x => new AboutPageItem
            {
                MenuItem = x,
                AboutItemType = AboutItemType.MenuItem
            })));

            aboutPageItems.Add(new Grouping<string, AboutPageItem>("Settings", new [] {
                new AboutPageItem
                {
                    AboutItemType = AboutItemType.Settings
                }
            }));

            return aboutPageItems;
        }

        public IList<Language> Languages => EnumExtension.GetEnumValues<Language>().ToList();

        public Language SelectedLanguage
        {
            get => this.selectedLanguage;
            set
            {
                if (this.SetProperty(ref this.selectedLanguage, value))
                {
                    Helpers.Settings.CurrentLanguage = this.selectedLanguage;
                    MessagingCenter.Send<object, CultureChangedMessage>(
                        this,
                        string.Empty,
                        new CultureChangedMessage(this.selectedLanguage.GetLanguageCode()));
                }
            }
        }

        public ICommand SelectedItemCommand => new Command<AboutPageItem>(async item => await HandleSelection(item));

        private async Task HandleSelection(AboutPageItem item)
        {
            switch (item.AboutItemType)
            {
                case AboutItemType.MenuItem:
                    switch (item.MenuItem.Parameter)
                    {
                        case "IssueTracker":
                            await ExecuteLaunchBrowserAsync(AboutThisApp.IssueTracker);
                            break;
                        case "TechnologyUsed":
                            await NavigationService.PushAsync(this.Navigation, new TechnologiesUsedPage());
                            break;
                        case "SaveSubscriptions":
                            Helpers.Settings.CommunitySubscriptions = AboutPageItems
                                .SelectMany(x => x.Items)
                                .Where(x => x.AboutItemType == AboutItemType.Subscriptions)
                                .Select(x => new CommunitySubscription
                                {
                                    IsSelected = x.Subscription.IsSelected,
                                    LoadedPosts = x.Subscription.LoadedPosts,
                                    CommunityName = x.Subscription.CommunityName,
                                    Type = x.Subscription.Type
                                }).ToList();
                                await Application.Current.MainPage.DisplayAlert(
                                string.Empty,
                                AppResources.SubscriptionsSavedMessage,
                                "OK");
                            break;
                        case "CreatedBy":
                            await Application.Current.MainPage.DisplayAlert(
                                "Credits",
                                AppResources.Credits,
                                "OK");
                            break;
                    }
                    break;
                case AboutItemType.Community:
                    await ExecuteLaunchBrowserAsync(item.Community.VkUrl);
                    break;
                case AboutItemType.Subscriptions:
                    switch (item.Subscription.Type)
                    {
                        case SocialMediaType.Vkontakte:
                            await ExecuteLaunchBrowserAsync(new Uri($"https://vk.com/{item.Subscription.CommunityName}"));
                            break;
                        case SocialMediaType.Twitter:
                            await ExecuteLaunchBrowserAsync(new Uri($"https://twitter.com/{item.Subscription.CommunityName}"));
                            break;
                        default:
                            break;
                    }
                    break;
                case AboutItemType.Text:
                    var toaster = DependencyService.Get<IToast>();
                    await Clipboard.SetTextAsync(item.Text);
                    toaster.SendToast(AppResources.Copied);
                    break;
                case AboutItemType.Friends:
                    await NavigationService.PushAsync(this.Navigation, new FriendsPage());
                    break;
            }
        }

        public string AppVersion
        {
            get
            {
                string buildNumber = string.Join(".", SplitInParts(VersionTracking.CurrentBuild, 4));

                string appVersion = $"{VersionTracking.CurrentVersion} ({buildNumber})";
                if (long.TryParse(VersionTracking.CurrentBuild, out var unixTimeSeconds))
                {
                    var datetime = DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds).ToLocalTime();
                    appVersion += $", {datetime.ToString("g")}";
                }

                return appVersion;
            }
        }

        private IEnumerable<string> SplitInParts(string s, int partLength)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (partLength <= 0)
            {
                throw new ArgumentException("Part length has to be positive.", "partLength");
            }

            for (var i = 0; i < s.Length; i += partLength)
            {
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
            }
        }

        public ICommand CopyAppVersionCommand => new Command(async () =>
        {
            Analytics.TrackEvent("App version copied", new Dictionary<string, string>() { ["AppVersion"] = AppVersion });

            var toaster = DependencyService.Get<IToast>();
            await Clipboard.SetTextAsync(AppVersion);
            toaster.SendToast(AppResources.Copied);
        });

        private void HandleLanguageChange()
        {
            this.OnPropertyChanged(nameof(this.AppVersion));

            AboutPageItems.ReplaceRange(GetAboutItems());
        }
    }
}
