namespace XamarinEvolve.Clients.UI
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Abstractions;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class RootPageiOS : TabbedPage
    {
        public RootPageiOS()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.Children.Add(new EvolveNavigationPage(new FeedPage()));
            this.Children.Add(new EvolveNavigationPage(new SpeakersPage()));

            this.Children.Add(new EvolveNavigationPage(new EventsPage()));
            if (FeatureFlags.SponsorsOnTabPage)
            {
                this.Children.Add(new EvolveNavigationPage(new SponsorsPage()));
            }

            this.Children.Add(new EvolveNavigationPage(new AboutPage()));

            MessagingService.Current.Subscribe<DeepLinkPage>(
                "DeepLinkPage",
                async (m, p) =>
                    {
                        switch (p.Page)
                        {
                            case AppPage.Notification:
                                this.Navigate(AppPage.Feed);
                                this.Navigate(AppPage.Notification);
                                await this.CurrentPage.Navigation.PopToRootAsync();
                                await this.CurrentPage.Navigation.PushAsync(new NotificationsPage());
                                break;
                            case AppPage.Events:
                                this.Navigate(AppPage.Events);
                                await this.CurrentPage.Navigation.PopToRootAsync();
                                break;
                            case AppPage.Session:
                                this.Navigate(AppPage.Sessions);
                                await this.CurrentPage.Navigation.PopToRootAsync();
                                var session = await DependencyService.Get<ISessionStore>().GetAppIndexSession(p.Id);
                                if (session == null)
                                {
                                    break;
                                }

                                await this.CurrentPage.Navigation.PushAsync(new TalkPage(session));
                                break;
                            case AppPage.Speaker:
                                this.Navigate(AppPage.Speakers);
                                await this.CurrentPage.Navigation.PopToRootAsync();

                                // TODO implement using Store

                                //var speaker = await DependencyService.Get<ISpeakerStore>().GetAppIndexSpeaker(p.Id);
                                //if (speaker == null)
                                //{
                                //    break;
                                //}

                                //ContentPage destination;
                                //if (Device.RuntimePlatform == Device.UWP)
                                //{
                                //    destination = new SpeakerDetailsPageUWP(speaker);
                                //}
                                //else
                                //{
                                //    destination = new SpeakerDetailsPage(speaker);
                                //}

                                //await this.CurrentPage.Navigation.PushAsync(destination);
                                break;
                        }
                    });
        }

        public void Navigate(AppPage menuId)
        {
            var page = this.Children.OfType<EvolveNavigationPage>().FirstOrDefault(
                n => n.CurrentPage is IProvidePageInfo && ((IProvidePageInfo)n.CurrentPage).PageType == menuId);

            if (page != null)
            {
                this.CurrentPage = page;
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
