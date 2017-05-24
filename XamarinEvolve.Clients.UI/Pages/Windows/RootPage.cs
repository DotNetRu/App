using Xamarin.Forms;
using System.Collections.Generic;
using XamarinEvolve.Clients.Portable;
using FormsToolkit;
using System.Collections.ObjectModel;
using MenuItem = XamarinEvolve.Clients.Portable.MenuItem;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
    public class RootPageWindows : MasterDetailPage
    {
        Dictionary<AppPage, Page> pages;
        MenuPageUWP menu;
        public static bool IsDesktop { get; set; }
        public RootPageWindows()
        {
            //MasterBehavior = MasterBehavior.Popover;
            pages = new Dictionary<AppPage, Page>();

			var items = new ObservableCollection<MenuItem>();


			items.Add(new MenuItem { Name = $"{EventInfo.EventName}", Icon = "menu_feed.png", Page = AppPage.Feed });
			items.Add(new MenuItem { Name = "Sessions", Icon = "menu_sessions.png", Page = AppPage.Sessions });
			if (FeatureFlags.SpeakersEnabled)
			{
				items.Add(new MenuItem { Name = "Speakers", Icon = "menu_speakers.png", Page = AppPage.Speakers });
			}
			if (FeatureFlags.EventsEnabled)
			{
				items.Add(new MenuItem { Name = "Events", Icon = "menu_events.png", Page = AppPage.Events });
			}
			if (FeatureFlags.SponsorsOnTabPage)
			{
				items.Add(new MenuItem { Name = "Sponsors", Icon = "menu_sponsors.png", Page = AppPage.Sponsors });
			}
			if (FeatureFlags.EvalEnabled)
			{
				items.Add(new MenuItem { Name = "Evaluations", Icon = "menu_evals.png", Page = AppPage.Evals });
			}

            items.Add(new MenuItem { Name = "Venue", Icon = "menu_venue.png", Page = AppPage.Venue });
			if (FeatureFlags.FloormapEnabled)
			{
				items.Add(new MenuItem { Name = "Floor Maps", Icon = "menu_plan.png", Page = AppPage.FloorMap });
			}
			if (FeatureFlags.ConferenceInformationEnabled)
			{
				items.Add(new MenuItem { Name = "Conference Info", Icon = "menu_info.png", Page = AppPage.ConferenceInfo });
			}
            items.Add(new MenuItem { Name = "Settings", Icon = "menu_settings.png", Page = AppPage.Settings });


            menu = new MenuPageUWP();
            menu.MenuList.ItemsSource = items;


            menu.MenuList.ItemSelected +=  (sender, args) =>
            {
                if (menu.MenuList.SelectedItem == null)
                    return;

                Device.BeginInvokeOnMainThread( () =>
                {
                    NavigateAsync(((MenuItem)menu.MenuList.SelectedItem).Page);
                    if (!IsDesktop)
                        IsPresented = false;
                });
            };

            Master = menu;
            NavigateAsync((int)AppPage.Feed);
            Title ="TechDays 2016";
        }



        public void NavigateAsync(AppPage menuId)
        {
            Page newPage = null;
            if (!pages.ContainsKey(menuId))
            {
                //only cache specific pages
                switch (menuId)
                {
                    case AppPage.Feed: //Feed
                        pages.Add(menuId, new EvolveNavigationPage(new FeedPage()));
                        break;
                    case AppPage.Sessions://sessions
                        pages.Add(menuId, new EvolveNavigationPage(new SessionsPage()));
                        break;
                    case AppPage.Speakers://sessions
                        pages.Add(menuId, new EvolveNavigationPage(new SpeakersPage()));
                        break;
                    case AppPage.Events://events
                        pages.Add(menuId, new EvolveNavigationPage(new EventsPage()));
                        break;
                    case AppPage.Sponsors://sponsors
                        newPage = new EvolveNavigationPage(new SponsorsPage());
                        break;
                    case AppPage.Evals: //venue
                        newPage = new EvolveNavigationPage(new EvaluationsPage());
                        break;
                    case AppPage.Venue: //venue
                        newPage = new EvolveNavigationPage(new VenuePage());
                        break;
                    case AppPage.ConferenceInfo://Conference info
                        newPage = new EvolveNavigationPage(new ConferenceInformationPage());
                        break;
                    case AppPage.FloorMap://Floor Maps
                        newPage = new EvolveNavigationPage(new FloorMapsCarouselPage());
                        break;
                    case AppPage.Settings://Settings
                        newPage = new EvolveNavigationPage(new SettingsPage());
                        break;
                }
            }

            if(newPage == null)
                newPage = pages[menuId];
            
            if(newPage == null)
                return;

            Detail = newPage;
            //await Navigation.PushAsync(newPage);
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


