namespace XamarinEvolve.Clients.UI
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataStore.Mock.Abstractions;
    using XamarinEvolve.Utils.Helpers;

    public class RootPageAndroid : MasterDetailPage
  {
    Dictionary<int, EvolveNavigationPage> pages;
    DeepLinkPage page;
    bool isRunning = false;

    public RootPageAndroid()
    {
        this.pages = new Dictionary<int, EvolveNavigationPage>();
        this.Master = new MenuPage(this);

        this.pages.Add(0, new EvolveNavigationPage(new FeedPage()));

        this.Detail = this.pages[0];
      MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
      {
          this.page = p;

        if (this.isRunning)
          await this.GoToDeepLink();
      });
    }



    public async Task NavigateAsync(int menuId)
    {
      EvolveNavigationPage newPage = null;
      if (!this.pages.ContainsKey(menuId))
      {
        // only cache specific pages
        switch (menuId)
        {
          case (int) AppPage.Feed: // Feed
              this.pages.Add(menuId, new EvolveNavigationPage(new FeedPage()));
            break;
          case (int) AppPage.Sessions: // sessions
              this.pages.Add(menuId, new EvolveNavigationPage(new MeetupPage()));
            break;
          case (int) AppPage.Speakers: // speakers
              this.pages.Add(menuId, new EvolveNavigationPage(new SpeakersPage()));
            break;
          case (int) AppPage.Events: // events
              this.pages.Add(menuId, new EvolveNavigationPage(new EventsPage()));
            break;
          case (int) AppPage.Sponsors: // sponsors
            newPage = new EvolveNavigationPage(new SponsorsPage());
            break;
          case (int) AppPage.Settings: // Settings
            newPage = new EvolveNavigationPage(new SettingsPage());
            break;
        }
      }

      if (newPage == null)
        newPage = this.pages[menuId];

      if (newPage == null)
        return;

      // if we are on the same tab and pressed it again.
      if (this.Detail == newPage)
      {
        await newPage.Navigation.PopToRootAsync();
      }

        this.Detail = newPage;
    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();


      if (Settings.Current.FirstRun)
      {
        MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
      }

        this.isRunning = true;

      await this.GoToDeepLink();

    }

    async Task GoToDeepLink()
    {
      if (this.page == null)
        return;
      var p = this.page.Page;
      var id = this.page.Id;
        this.page = null;
      switch (p)
      {

        case AppPage.Session:
          await this.NavigateAsync((int) AppPage.Sessions);
          var session = await DependencyService.Get<ISessionStore>().GetAppIndexSession(id);
          if (session == null)
            break;
          await this.Detail.Navigation.PushAsync(new TalkPage(session));
          break;
        case AppPage.Speaker:
          await this.NavigateAsync((int) AppPage.Speakers);
          var speaker = await DependencyService.Get<ISpeakerStore>().GetAppIndexSpeaker(id);
          if (speaker == null)
            break;

          ContentPage destination;
          if (Device.RuntimePlatform == Device.UWP)
          {
            destination = new SpeakerDetailsPageUWP(speaker);
          }
          else
          {
            destination = new SpeakerDetailsPage(speaker);
          }

          await this.Detail.Navigation.PushAsync(destination);
          break;
      }
    }
  }
}


