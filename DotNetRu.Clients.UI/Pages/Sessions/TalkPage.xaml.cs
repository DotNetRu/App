namespace DotNetRu.Clients.UI.Pages.Sessions
{
    using System.Linq;

    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Speakers;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    using Xamarin.Forms;

    public partial class TalkPage
    {
        private readonly IPlatformSpecificExtension<TalkModel> extension;

        private TalkViewModel talkViewModel;

        /// <summary>
        /// Design-time only
        /// </summary>
        public TalkPage() : this(TalkService.Talks.First())
        {
        }

        public TalkPage(TalkModel talkModel)
        {
            this.InitializeComponent();

            this.extension = DependencyService.Get<IPlatformSpecificExtension<TalkModel>>();

            this.ItemId = talkModel?.Title;

            this.ListViewSpeakers.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewSpeakers.SelectedItem is SpeakerModel speaker))
                    {
                        return;
                    }

                    ContentPage destination;

                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        var speakerDetailsUwp =
                            new SpeakerDetailsPageUWP(this.talkViewModel.TalkModel.Id) { SpeakerModel = speaker };
                        destination = speakerDetailsUwp;
                    }
                    else
                    {
                        var speakerDetails =
                            new SpeakerDetailsPage() { SpeakerModel = speaker };
                        destination = speakerDetails;
                    }

                    await NavigationService.PushAsync(this.Navigation, destination);
                    this.ListViewSpeakers.SelectedItem = null;
                };

            this.BindingContext = new TalkViewModel(this.Navigation, talkModel);
        }

        public override AppPage PageType => AppPage.Talk;

        public TalkViewModel ViewModel => this.talkViewModel ?? (this.talkViewModel = this.BindingContext as TalkViewModel);

        public void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.ListViewSpeakers.ItemTapped += this.ListViewTapped;

            var count = this.ViewModel?.TalkModel?.Speakers?.Count ?? 0;
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -count + 1;
            if ((this.ViewModel?.TalkModel?.Speakers?.Count ?? 0) > 0)
            {
                this.ListViewSpeakers.HeightRequest = (count * this.ListViewSpeakers.RowHeight) - adjust;
            }

            if (this.extension != null)
            {
                await this.extension.Execute(this.ViewModel.TalkModel);
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewSpeakers.ItemTapped -= this.ListViewTapped;

            if (this.extension != null)
            {
                await this.extension.Finish();
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.talkViewModel = null;

            this.ListViewSpeakers.HeightRequest = 0;

            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.ViewModel.SessionMaterialItems.Count + 2;
            this.ListViewSessionMaterial.HeightRequest =
                (this.ViewModel.SessionMaterialItems.Count * this.ListViewSessionMaterial.RowHeight) - adjust;
        }
    }
}

