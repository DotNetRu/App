namespace DotNetRu.Clients.UI.Pages.Home
{
    using System;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Controls;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using Xamarin.Forms;

    public partial class NewsPage
    {
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
            this.ListViewSocial.ItemSelected += (sender, e) => this.ListViewSocial.SelectedItem = null;
        }

        public override AppPage PageType => AppPage.Feed;

        public NewsViewModel NewsViewModel => this.newsViewModel ?? (this.newsViewModel = this.BindingContext as NewsViewModel);

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

        private void UpdatePage()
        {
            bool forceRefresh = DateTime.UtcNow > (this.NewsViewModel?.NextForceRefresh ?? DateTime.UtcNow);

            this.newsViewModel.EvaluateVisualState();

            if (forceRefresh)
            {
                this.NewsViewModel?.RefreshCommand.Execute(null);
            }
            else
            {
                if (this.NewsViewModel?.Tweets.Count == 0)
                {
                    this.NewsViewModel.LoadSocialCommand.Execute(null);
                }

                // if (this.firstLoad && this.NewsViewModel?.Sessions.Count == 0)
                // {
                // this.firstLoad = false;
                // this.NewsViewModel.LoadSessionsCommand.Execute(null);
                // }
            }
        }
    }
}