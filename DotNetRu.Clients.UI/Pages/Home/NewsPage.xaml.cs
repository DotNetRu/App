namespace XamarinEvolve.Clients.UI
{
    using System;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.Portable.ViewModel;
    using XamarinEvolve.Utils.Helpers;

    public partial class NewsPage
    {
        public override AppPage PageType => AppPage.Feed;

        public NewsViewModel NewsViewModel => this.newsViewModel ?? (this.newsViewModel = this.BindingContext as NewsViewModel);

        private NewsViewModel newsViewModel;

        public NewsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new NewsViewModel();

            if (Device.RuntimePlatform == Device.UWP)
            {
                this.ToolbarItems.Add(
                    new ToolbarItem
                        {
                            Text = "Refresh",
                            Icon = "toolbar_refresh.png",
                            Command = this.NewsViewModel.RefreshCommand
                        });
            }

            this.NewsViewModel.Tweets.CollectionChanged += (sender, e) =>
                {
                    var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.NewsViewModel.Tweets.Count + 2;
                    this.ListViewSocial.HeightRequest = (this.NewsViewModel.Tweets.Count * this.ListViewSocial.RowHeight) - adjust;
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
            bool forceRefresh = DateTime.UtcNow > (this.NewsViewModel?.NextForceRefresh ?? DateTime.UtcNow);

            this.newsViewModel.EvaluateVisualState();

            if (forceRefresh)
            {
                this.NewsViewModel.RefreshCommand.Execute(null);
            }
            else
            {

                if (this.NewsViewModel.Tweets.Count == 0)
                {
                    this.NewsViewModel.LoadSocialCommand.Execute(null);
                }

                if (this.firstLoad && this.NewsViewModel.Sessions.Count == 0)
                {
                    this.firstLoad = false;
                    this.NewsViewModel.LoadSessionsCommand.Execute(null);
                }

                if (this.NewsViewModel.Notification == null) this.NewsViewModel.LoadNotificationsCommand.Execute(null);
            }

        }


        public void OnResume()
        {
            this.UpdatePage();
        }

    }
}

