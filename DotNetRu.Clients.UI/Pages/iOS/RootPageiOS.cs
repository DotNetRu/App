using System.Linq;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.UI.Controls;
using DotNetRu.Clients.UI.Pages.Events;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.Clients.UI.Pages.Home;
using DotNetRu.Clients.UI.Pages.Info;
using DotNetRu.Clients.UI.Pages.Speakers;
using DotNetRu.Utils.Helpers;
using FormsToolkit;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace DotNetRu.Clients.UI.Pages.iOS
{
    public class RootPageiOS : TabbedPage
    {
        public RootPageiOS()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.Children.Add(new EvolveNavigationPage(new NewsPage()));
            this.Children.Add(new EvolveNavigationPage(new SpeakersPage()));

            this.Children.Add(new EvolveNavigationPage(new MeetupsPage()));
            if (FeatureFlags.SponsorsOnTabPage)
            {
                this.Children.Add(new EvolveNavigationPage(new FriendsPage()));
            }

            this.Children.Add(new EvolveNavigationPage(new SettingsPage()));

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
                            case AppPage.Meetups:
                                this.Navigate(AppPage.Meetups);
                                await this.CurrentPage.Navigation.PopToRootAsync();
                                break;
                            case AppPage.Talk:
                                this.Navigate(AppPage.Meetup);
                                await this.CurrentPage.Navigation.PopToRootAsync();

                                break;
                            case AppPage.Speaker:
                                this.Navigate(AppPage.Speakers);
                                await this.CurrentPage.Navigation.PopToRootAsync();

                                // TODO implement using Store

                                // var speaker = await DependencyService.Get<ISpeakerStore>().GetAppIndexSpeaker(p.Id);
                                // if (speaker == null)
                                // {
                                // break;
                                // }

                                // ContentPage destination;
                                // if (Device.RuntimePlatform == Device.UWP)
                                // {
                                // destination = new SpeakerDetailsPageUWP(speaker);
                                // }
                                // else
                                // {
                                // destination = new SpeakerDetailsPage(speaker);
                                // }

                                // await this.CurrentPage.Navigation.PushAsync(destination);
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
