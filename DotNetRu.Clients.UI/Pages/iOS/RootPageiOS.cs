namespace XamarinEvolve.Clients.UI
{
    using System.Linq;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataStore.Mock.Abstractions;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class RootPageiOS : TabbedPage
    {

        public RootPageiOS()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Children.Add(new EvolveNavigationPage(new FeedPage()));
                Children.Add(new EvolveNavigationPage(new SpeakersPage()));

            Children.Add(new EvolveNavigationPage(new EventsPage()));
            if (FeatureFlags.SponsorsOnTabPage)
            {
                Children.Add(new EvolveNavigationPage(new SponsorsPage()));
            }
            Children.Add(new EvolveNavigationPage(new AboutPage()));

            MessagingService.Current.Subscribe<DeepLinkPage>(
                "DeepLinkPage",
                async (m, p) =>
                    {
                        switch (p.Page)
                        {
                            case AppPage.Notification:
                                Navigate(AppPage.Feed);
                                Navigate(AppPage.Notification);
                                await CurrentPage.Navigation.PopToRootAsync();
                                await CurrentPage.Navigation.PushAsync(new NotificationsPage());
                                break;
                            case AppPage.Events:
                                Navigate(AppPage.Events);
                                await CurrentPage.Navigation.PopToRootAsync();
                                break;
                            case AppPage.Session:
                                Navigate(AppPage.Sessions);
                                await CurrentPage.Navigation.PopToRootAsync();
                                var session = await DependencyService.Get<ISessionStore>().GetAppIndexSession(p.Id);
                                if (session == null) break;
                                await CurrentPage.Navigation.PushAsync(new SessionDetailsPage(session));
                                break;
                            case AppPage.Speaker:
                                Navigate(AppPage.Speakers);
                                await CurrentPage.Navigation.PopToRootAsync();
                                var speaker = await DependencyService.Get<ISpeakerStore>().GetAppIndexSpeaker(p.Id);
                                if (speaker == null) break;

                                ContentPage destination;
                                if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                                {
                                    destination = new SpeakerDetailsPageUWP(speaker);
                                }
                                else
                                {
                                    destination = new SpeakerDetailsPage(speaker);
                                }
                                await CurrentPage.Navigation.PushAsync(destination);
                                break;
                        }
                    });
        }

        public void Navigate(AppPage menuId)
        {
            var page = Children.OfType<EvolveNavigationPage>().FirstOrDefault(
                n => n.CurrentPage is IProvidePageInfo && ((IProvidePageInfo)n.CurrentPage).PageType == menuId);

            if (page != null)
            {
                CurrentPage = page;
            }
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


