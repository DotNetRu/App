using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using XamarinEvolve.Clients.Portable;
using FormsToolkit;

using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
    using XamarinEvolve.DataStore.Mock.Abstractions;
    using XamarinEvolve.Utils.Helpers;

	public class RootPageAndroid : MasterDetailPage
  {
    Dictionary<int, EvolveNavigationPage> pages;
    DeepLinkPage page;
    bool isRunning = false;

    public RootPageAndroid()
    {
      pages = new Dictionary<int, EvolveNavigationPage>();
      Master = new MenuPage(this);

      pages.Add(0, new EvolveNavigationPage(new FeedPage()));

      Detail = pages[0];
      MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
      {
        page = p;

        if (isRunning)
          await GoToDeepLink();
      });
    }



    public async Task NavigateAsync(int menuId)
    {
      EvolveNavigationPage newPage = null;
      if (!pages.ContainsKey(menuId))
      {
        //only cache specific pages
        switch (menuId)
        {
          case (int) AppPage.Feed: //Feed
            pages.Add(menuId, new EvolveNavigationPage(new FeedPage()));
            break;
          case (int) AppPage.Sessions: //sessions
            pages.Add(menuId, new EvolveNavigationPage(new SessionsPage()));
            break;
          case (int) AppPage.Speakers: //speakers
            pages.Add(menuId, new EvolveNavigationPage(new SpeakersPage()));
            break;
          case (int) AppPage.Events: //events
            pages.Add(menuId, new EvolveNavigationPage(new EventsPage()));
            break;
          case (int) AppPage.Sponsors: //sponsors
            newPage = new EvolveNavigationPage(new SponsorsPage());
            break;
          case (int) AppPage.Settings: //Settings
            newPage = new EvolveNavigationPage(new SettingsPage());
            break;
        }
      }

      if (newPage == null)
        newPage = pages[menuId];

      if (newPage == null)
        return;

      //if we are on the same tab and pressed it again.
      if (Detail == newPage)
      {
        await newPage.Navigation.PopToRootAsync();
      }

      Detail = newPage;
    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();


      if (Settings.Current.FirstRun)
      {
        MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
      }

      isRunning = true;

      await GoToDeepLink();

    }

    async Task GoToDeepLink()
    {
      if (page == null)
        return;
      var p = page.Page;
      var id = page.Id;
      page = null;
      switch (p)
      {

        case AppPage.Session:
          await NavigateAsync((int) AppPage.Sessions);
          var session = await DependencyService.Get<ISessionStore>().GetAppIndexSession(id);
          if (session == null)
            break;
          await Detail.Navigation.PushAsync(new SessionDetailsPage(session));
          break;
        case AppPage.Speaker:
          await NavigateAsync((int) AppPage.Speakers);
          var speaker = await DependencyService.Get<ISpeakerStore>().GetAppIndexSpeaker(id);
          if (speaker == null)
            break;

          ContentPage destination;
          if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
          {
            destination = new SpeakerDetailsPageUWP(speaker);
          }
          else
          {
            destination = new SpeakerDetailsPage(speaker);
          }
          await Detail.Navigation.PushAsync(destination);
          break;
      }
    }
  }
}


