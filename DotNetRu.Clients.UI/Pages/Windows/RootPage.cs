namespace XamarinEvolve.Clients.UI
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    using MenuItem = XamarinEvolve.Clients.Portable.MenuItem;

    public class RootPageWindows : MasterDetailPage
    {
        Dictionary<AppPage, Page> pages;

        MenuPageUWP menu;

        public static bool IsDesktop { get; set; }

        public RootPageWindows()
        {
            // MasterBehavior = MasterBehavior.Popover;
            this.pages = new Dictionary<AppPage, Page>();

            var items = new ObservableCollection<MenuItem>();


            items.Add(new MenuItem { Name = $"{AboutThisApp.AppName}", Icon = "menu_feed.png", Page = AppPage.Feed });
            items.Add(new MenuItem { Name = "Sessions", Icon = "menu_sessions.png", Page = AppPage.Sessions });
            items.Add(new MenuItem { Name = "Speakers", Icon = "menu_speakers.png", Page = AppPage.Speakers });

            items.Add(new MenuItem { Name = "Events", Icon = "menu_events.png", Page = AppPage.Events });
            if (FeatureFlags.SponsorsOnTabPage)
            {
                items.Add(new MenuItem { Name = "Sponsors", Icon = "menu_sponsors.png", Page = AppPage.Sponsors });
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
                        this.pages.Add(menuId, new EvolveNavigationPage(new FeedPage()));
                        break;
                    case AppPage.Sessions: // sessions
                        this.pages.Add(menuId, new EvolveNavigationPage(new MeetupPage()));
                        break;
                    case AppPage.Speakers: // sessions
                        this.pages.Add(menuId, new EvolveNavigationPage(new SpeakersPage()));
                        break;
                    case AppPage.Events: // events
                        this.pages.Add(menuId, new EvolveNavigationPage(new EventsPage()));
                        break;
                    case AppPage.Sponsors: // sponsors
                        newPage = new EvolveNavigationPage(new SponsorsPage());
                        break;
                    case AppPage.Settings: // Settings
                        newPage = new EvolveNavigationPage(new SettingsPage());
                        break;
                }
            }

            if (newPage == null) newPage = this.pages[menuId];

            if (newPage == null) return;

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


