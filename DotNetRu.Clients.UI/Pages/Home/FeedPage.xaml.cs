using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using FormsToolkit;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	using XamarinEvolve.Utils.Helpers;

	public partial class FeedPage : BasePage
	{
		public override AppPage PageType => AppPage.Feed;

        FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);

		FeedViewModel vm;
        DateTime favoritesTime;
        string loggedIn;
        public FeedPage()
        {
            InitializeComponent();
            loggedIn = Settings.Current.UserIdentifier;
            BindingContext = new FeedViewModel();

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon = "toolbar_refresh.png",
                    Command = ViewModel.RefreshCommand
                });
            }

            favoritesTime = Settings.Current.LastFavoriteTime;
            ViewModel.Tweets.CollectionChanged += (sender, e) => 
                {
                    var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Tweets.Count + 2;
                    ListViewSocial.HeightRequest = (ViewModel.Tweets.Count * ListViewSocial.RowHeight)  - adjust;
                };

            ViewModel.Sessions.CollectionChanged += (sender, e) => 
                {
                    var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Sessions.Count + 1;
                    ListViewSessions.HeightRequest = (ViewModel.Sessions.Count * ListViewSessions.RowHeight) - adjust;
                };

            ListViewSessions.ItemTapped += (sender, e) => ListViewSessions.SelectedItem = null;
            ListViewSessions.ItemSelected += async (sender, e) => 
                {
                    var session = ListViewSessions.SelectedItem as Session;
                    if(session == null)
                        return;
                    var sessionDetails = new SessionDetailsPage(session);

                    await NavigationService.PushAsync(Navigation, sessionDetails);
                    ListViewSessions.SelectedItem = null;
                }; 

            NotificationStack.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => 
                        {
                            await NavigationService.PushAsync(Navigation, new NotificationsPage());
                        })
                });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdatePage();

            MessagingService.Current.Subscribe<string>(MessageKeys.NavigateToImage, async (m, image) =>
                {
                    await NavigationService.PushModalAsync(Navigation, new EvolveNavigationPage(new TweetImagePage(image)));
                });

			MessagingService.Current.Subscribe(MessageKeys.NavigateToConferenceFeedback, async (m) =>
				{
					await NavigationService.PushModalAsync(Navigation, new EvolveNavigationPage(new ConferenceFeedbackPage()));
				});
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingService.Current.Unsubscribe<string>(MessageKeys.NavigateToImage);
        }

        bool firstLoad = true;
        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow)) ||
                    loggedIn != Settings.Current.UserIdentifier;

            loggedIn = Settings.Current.UserIdentifier;

			vm.EvaluateVisualState();

            if (forceRefresh)
            {
                ViewModel.RefreshCommand.Execute(null);
                favoritesTime = Settings.Current.LastFavoriteTime;
            }
            else
            {

                if (ViewModel.Tweets.Count == 0)
                {

                    ViewModel.LoadSocialCommand.Execute(null);
                }

                if ((firstLoad && ViewModel.Sessions.Count == 0) || favoritesTime != Settings.Current.LastFavoriteTime)
                {
                    if (firstLoad)
                        Settings.Current.LastFavoriteTime = DateTime.UtcNow;
                    
                    firstLoad = false;
                    favoritesTime = Settings.Current.LastFavoriteTime;
                    ViewModel.LoadSessionsCommand.Execute(null);
                }

                if (ViewModel.Notification == null)
                    ViewModel.LoadNotificationsCommand.Execute(null);
            }

        }


        public void OnResume()
        {
            UpdatePage();
        }

    }
}

