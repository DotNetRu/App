using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Handlers;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Speakers;
using DotNetRu.DataStore.Audit.Models;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Speakers
{
    public partial class SpeakersPage
    {
        public override AppPage PageType => AppPage.Speakers;

        SpeakersViewModel speakersViewModel;

        SpeakersViewModel ViewModel => this.speakersViewModel ??= this.BindingContext as SpeakersViewModel;

        public SpeakersPage()
        {
            this.InitializeComponent();
            this.BindingContext = new SpeakersViewModel(this.Navigation);

            Shell.SetSearchHandler(this, new SpeakersSearchHandler(this.BindingContext as SpeakersViewModel, this)
            {
                Placeholder = (this.BindingContext as SpeakersViewModel)?.Resources["Enter speaker fullname"] ?? "Enter speaker fullname",
                ShowsResults = true,
                DisplayMemberName = nameof(SpeakerModel.FullName),
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { VerticalOptions = LayoutOptions.Center, Padding = 16.5, ColumnSpacing = (double)Application.Current.Resources["StandardSpacing"] };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var circleImage = new CircleImage
                    {
                        FillColor = (Color)Application.Current.Resources["Primary"],
                        VerticalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFill,
                        HeightRequest = 44,
                        WidthRequest = 44
                    };
                    circleImage.SetBinding(Image.SourceProperty, nameof(SpeakerModel.AvatarSmallURL));

                    var stackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Spacing = (double)Application.Current.Resources["SmallSpacing"]
                    };
                    var fullNameLabel = new Label { Style = (Style)Application.Current.Resources["EvolveListItemTextStyle"] };
                    fullNameLabel.SetBinding(Label.TextProperty, nameof(SpeakerModel.FullName));
                    stackLayout.Children.Add(fullNameLabel);
                    var titleLabel = new Label { Style = (Style)Application.Current.Resources["EvolveListItemDetailTextStyle"] };
                    titleLabel.SetBinding(Label.TextProperty, nameof(SpeakerModel.Title));
                    stackLayout.Children.Add(titleLabel);

                    grid.Children.Add(circleImage);
                    grid.Children.Add(stackLayout, 1, 0);
                    return grid;
                })
            });

            if (Device.RuntimePlatform == Device.Android)
            {
                this.ListViewSpeakers.Effects.Add(Effect.Resolve("Xpirit.ListViewSelectionOnTopEffect"));
            }

            this.ListViewSpeakers.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewSpeakers.SelectedItem is SpeakerModel speaker))
                    {
                        return;
                    }

                    ContentPage destination = new SpeakerDetailsPage(speaker);

                    await NavigationService.PushAsync(this.Navigation, destination);
                    this.ListViewSpeakers.SelectedItem = null;
                };
        }

        public void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewSpeakers.ItemTapped += this.ListViewTapped;
            if (this.ViewModel.Speakers?.Count == 0)
            {
                this.ViewModel.LoadSpeakersCommand.Execute(true);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewSpeakers.ItemTapped -= this.ListViewTapped;
        }
    }
}
