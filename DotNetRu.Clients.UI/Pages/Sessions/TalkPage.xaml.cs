namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.DataObjects;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataObjects;

    public partial class TalkPage
    {
        private readonly IPlatformSpecificExtension<TalkModel> extension;

        public override AppPage PageType => AppPage.Session;

        public TalkViewModel ViewModel => this.talkViewModel ?? (this.talkViewModel = this.BindingContext as TalkViewModel);

        private TalkViewModel talkViewModel;

        public TalkPage(TalkModel talkModel)
        {
            this.InitializeComponent();

            this.extension = DependencyService.Get<IPlatformSpecificExtension<TalkModel>>();

            this.ItemId = talkModel?.Title;

            this.ListViewSpeakers.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewSpeakers.SelectedItem is Speaker speaker))
                    {
                        return;
                    }

                    ContentPage destination;

                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        var speakerDetailsUwp =
                            new SpeakerDetailsPageUWP(this.talkViewModel.TalkModel.Id) { Speaker = speaker };
                        destination = speakerDetailsUwp;
                    }
                    else
                    {
                        var speakerDetails =
                            new SpeakerDetailsPage(this.talkViewModel.TalkModel.Id) { Speaker = speaker };
                        destination = speakerDetails;
                    }

                    await NavigationService.PushAsync(this.Navigation, destination);
                    this.ListViewSpeakers.SelectedItem = null;
                };

            this.ButtonRate.Clicked += async (sender, e) =>
                {
                    await NavigationService.PushModalAsync(
                        this.Navigation,
                        new EvolveNavigationPage(new FeedbackPage(this.ViewModel.TalkModel)));
                };
            this.BindingContext = new TalkViewModel(this.Navigation, talkModel);
            this.ViewModel.LoadSessionCommand.Execute(null);

        }

        public void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        public void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            this.Title = e.ScrollY > this.SessionDate.Y ? this.ViewModel.TalkModel.ShortTitle : "Talk";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.MainScroll.Scrolled += this.MainScroll_Scrolled;
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
            this.MainScroll.Scrolled -= this.MainScroll_Scrolled;
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

