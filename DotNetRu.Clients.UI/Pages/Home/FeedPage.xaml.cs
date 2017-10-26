namespace XamarinEvolve.Clients.UI
{
    using System;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.Portable.ViewModel;
    using XamarinEvolve.Utils.Helpers;

    public partial class FeedPage
    {
        public override AppPage PageType => AppPage.Feed;

        FeedViewModel ViewModel => this.feedViewModel ?? (this.feedViewModel = this.BindingContext as FeedViewModel);

        FeedViewModel feedViewModel;

        public FeedPage()
        {
            this.InitializeComponent();
            this.BindingContext = new FeedViewModel();

            if (Device.RuntimePlatform == Device.UWP)
            {
                this.ToolbarItems.Add(
                    new ToolbarItem
                        {
                            Text = "Refresh",
                            Icon = "toolbar_refresh.png",
                            Command = this.ViewModel.RefreshCommand
                        });
            }

            this.ViewModel.Tweets.CollectionChanged += (sender, e) =>
                {
                    var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.ViewModel.Tweets.Count + 2;
                    this.ListViewSocial.HeightRequest = (this.ViewModel.Tweets.Count * this.ListViewSocial.RowHeight) - adjust;
                };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.UpdatePage();

            MessagingService.Current.Subscribe<string>(
                MessageKeys.NavigateToImage,
                async (m, image) =>
                    {
                        await NavigationService.PushModalAsync(
                            this.Navigation,
                            new EvolveNavigationPage(new TweetImagePage(image)));
                    });

            MessagingService.Current.Subscribe(
                MessageKeys.NavigateToConferenceFeedback,
                async (m) =>
                    {
                        await NavigationService.PushModalAsync(
                            this.Navigation,
                            new EvolveNavigationPage(new ConferenceFeedbackPage()));
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
            bool forceRefresh = DateTime.UtcNow > (this.ViewModel?.NextForceRefresh ?? DateTime.UtcNow);

            this.feedViewModel.EvaluateVisualState();

            if (forceRefresh)
            {
                this.ViewModel.RefreshCommand.Execute(null);
            }
            else
            {

                if (this.ViewModel.Tweets.Count == 0)
                {
                    this.ViewModel.LoadSocialCommand.Execute(null);
                }

                if (this.firstLoad && this.ViewModel.Sessions.Count == 0)
                {
                    this.firstLoad = false;
                    this.ViewModel.LoadSessionsCommand.Execute(null);
                }

                if (this.ViewModel.Notification == null) this.ViewModel.LoadNotificationsCommand.Execute(null);
            }

        }


        public void OnResume()
        {
            this.UpdatePage();
        }

    }
}

