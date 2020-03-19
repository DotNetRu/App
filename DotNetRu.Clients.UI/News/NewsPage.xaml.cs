using DotNetRu.Clients.UI.Handlers;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Essentials;

namespace DotNetRu.Clients.UI.Pages.Home
{
    using System;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
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

            Shell.SetSearchHandler(this, new NewsSearchHandler(this.BindingContext as NewsViewModel, this)
            {
                Placeholder = (this.BindingContext as NewsViewModel)?.Resources["Enter search term"] ?? "Enter search term",
                ShowsResults = true,
                DisplayMemberName = nameof(Tweet.Text),
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { VerticalOptions = LayoutOptions.Center, Padding = 16.5, ColumnSpacing = (double)Application.Current.Resources["StandardSpacing"] };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var circleImage = new CircleImage
                    {
                        FillColor = (Color)Application.Current.Resources["Primary"],
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFill,
                        HeightRequest = 44,
                        WidthRequest = 44
                    };
                    circleImage.SetBinding(Image.SourceProperty, nameof(Tweet.Image));

                    var stackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Spacing = (double)Application.Current.Resources["SmallSpacing"]
                    };
                    var tweetNameLabel = new Label { Style = (Style)Application.Current.Resources["EvolveListItemTextStyle"] };
                    tweetNameLabel.SetBinding(Label.TextProperty, nameof(Tweet.Name));
                    stackLayout.Children.Add(tweetNameLabel);
                    var tweetDateDisplayLabel = new Label { Style = (Style)Application.Current.Resources["EvolveListItemDetailTextStyle"] };
                    tweetDateDisplayLabel.SetBinding(Label.TextProperty, nameof(Tweet.DateDisplay));
                    stackLayout.Children.Add(tweetDateDisplayLabel);
                    var tweetTextLabel = new Label { Style = (Style)Application.Current.Resources["EvolveListItemDetailTextStyle"] };
                    tweetTextLabel.SetBinding(Label.TextProperty, nameof(Tweet.Text));
                    stackLayout.Children.Add(tweetTextLabel);

                    grid.Children.Add(circleImage);
                    grid.Children.Add(stackLayout, 1, 0);
                    return grid;
                })
            });

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
            this.ListViewSocial.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem is Tweet tweet && !string.IsNullOrWhiteSpace(tweet.Url))
                {
                    await Launcher.OpenAsync(new Uri(tweet.Url));
                }
                this.ListViewSocial.SelectedItem = null;
            };
        }

        public override AppPage PageType => AppPage.News;

        public NewsViewModel NewsViewModel => this.newsViewModel ??= this.BindingContext as NewsViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.UpdatePage();
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
