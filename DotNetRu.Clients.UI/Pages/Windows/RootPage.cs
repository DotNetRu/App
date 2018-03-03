namespace DotNetRu.Clients.UI.Pages.Windows
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.UI.Controls;
    using DotNetRu.Clients.UI.Pages.Events;
    using DotNetRu.Clients.UI.Pages.Friends;
    using DotNetRu.Clients.UI.Pages.Home;
    using DotNetRu.Clients.UI.Pages.Info;
    using DotNetRu.Clients.UI.Pages.Sessions;
    using DotNetRu.Clients.UI.Pages.Speakers;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    using MenuItem = DotNetRu.Clients.Portable.Model.MenuItem;

    public class RootPageWindows : MasterDetailPage
    {
        readonly Dictionary<AppPage, Page> pages;

        readonly MenuPageUWP menu;

        public static bool IsDesktop { get; set; }

        public RootPageWindows()
        {
            // MasterBehavior = MasterBehavior.Popover;
            this.pages = new Dictionary<AppPage, Page>();

            var items = new ObservableCollection<MenuItem>();


            items.Add(new MenuItem { Name = $"{AboutThisApp.AppName}", Icon = "menu_feed.png", Page = AppPage.Feed });
            items.Add(new MenuItem { Name = "Sessions", Icon = "menu_sessions.png", Page = AppPage.Meetup });
            items.Add(new MenuItem { Name = "Speakers", Icon = "menu_speakers.png", Page = AppPage.Speakers });

            items.Add(new MenuItem { Name = "Meetups", Icon = "menu_events.png", Page = AppPage.Meetups });
            if (FeatureFlags.SponsorsOnTabPage)
            {
                items.Add(new MenuItem { Name = "Friends", Icon = "menu_sponsors.png", Page = AppPage.Friends });
            }

            items.Add(new MenuItem { Name = "About", Icon = "menu_info.png", Page = AppPage.Settings });

            this.menu = new MenuPageUWP();
            this.menu.MenuList.ItemsSource = items;

            this.menu.MenuList.ItemSelected += (sender, args) =>
                {
                    if (this.menu.MenuList.SelectedItem == null) return;

                    Device.BeginInvokeOnMainThread(
                        () =>
                            {
                                this.NavigateAsync(((MenuItem)this.menu.MenuList.SelectedItem).Page);
                                if (!IsDesktop) this.IsPresented = false;
                            });
                };

            this.Master = this.menu;
            this.NavigateAsync((int)AppPage.Feed);
            this.Title = "DotNetRu App";
        }



        public void NavigateAsync(AppPage menuId)
        {
            Page newPage = null;
            if (!this.pages.ContainsKey(menuId))
            {
                // only cache specific pages
                switch (menuId)
                {
                    case AppPage.Feed: // Feed
                        this.pages.Add(menuId, new NavigationPage(new NewsPage()));
                        break;
                    case AppPage.Meetup: // sessions
                        this.pages.Add(menuId, new NavigationPage(new MeetupPage()));
                        break;
                    case AppPage.Speakers: // sessions
                        this.pages.Add(menuId, new NavigationPage(new SpeakersPage()));
                        break;
                    case AppPage.Meetups: // events
                        this.pages.Add(menuId, new NavigationPage(new MeetupsPage()));
                        break;
                    case AppPage.Friends: // sponsors
                        newPage = new NavigationPage(new FriendsPage());
                        break;
                    case AppPage.Settings: // Settings
                        newPage = new NavigationPage(new SettingsPage());
                        break;
                }
            }

            if (newPage == null)
            {
                newPage = this.pages[menuId];
            }

            if (newPage == null)
            {
                return;
            }

            this.Detail = newPage;

            // await Navigation.PushAsync(newPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Settings.Current.FirstRun)
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
        }
    }
}


