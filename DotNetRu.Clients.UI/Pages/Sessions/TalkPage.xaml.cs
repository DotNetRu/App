namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.DataObjects;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataObjects;

    public partial class SessionDetailsPage
    {
        private IPlatformSpecificExtension<Session> _extension;

        public override AppPage PageType => AppPage.Session;

        TalkViewModel ViewModel => this.talkViewModel ?? (this.talkViewModel = this.BindingContext as TalkViewModel);

        TalkViewModel talkViewModel;

        public SessionDetailsPage(Session session)
        {
            this.InitializeComponent();

            this._extension = DependencyService.Get<IPlatformSpecificExtension<Session>>();

            this.ItemId = session?.Title;

            this.ListViewSpeakers.ItemSelected += async (sender, e) =>
                {
                    var speaker = this.ListViewSpeakers.SelectedItem as Speaker;
                    if (speaker == null) return;

                    ContentPage destination;

                    if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                    {
                        var speakerDetailsUwp = new SpeakerDetailsPageUWP(this.talkViewModel.Session.Id);
                        speakerDetailsUwp.Speaker = speaker;
                        destination = speakerDetailsUwp;
                    }
                    else
                    {
                        var speakerDetails = new SpeakerDetailsPage(this.talkViewModel.Session.Id);
                        speakerDetails.Speaker = speaker;
                        destination = speakerDetails;
                    }

                    await NavigationService.PushAsync(this.Navigation, destination);
                    this.ListViewSpeakers.SelectedItem = null;
                };

            this.ButtonRate.Clicked += async (sender, e) =>
                {
                    await NavigationService.PushModalAsync(
                        this.Navigation,
                        new EvolveNavigationPage(new FeedbackPage(this.ViewModel.Session)));
                };
            this.BindingContext = new TalkViewModel(this.Navigation, session);
            this.ViewModel.LoadSessionCommand.Execute(null);

        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY > this.SessionDate.Y)
            {
                this.Title = this.ViewModel.Session.ShortTitle;
            }
            else
            {
                this.Title = "Talk";
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.MainScroll.Scrolled += this.MainScroll_Scrolled;
            this.ListViewSpeakers.ItemTapped += this.ListViewTapped;

            var count = this.ViewModel?.Session?.Speakers?.Count ?? 0;
            var adjust = Device.OS != TargetPlatform.Android ? 1 : -count + 1;
            if ((this.ViewModel?.Session?.Speakers?.Count ?? 0) > 0) this.ListViewSpeakers.HeightRequest = (count * this.ListViewSpeakers.RowHeight) - adjust;

            if (this._extension != null)
            {
                await this._extension.Execute(this.ViewModel.Session);
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            this.MainScroll.Scrolled -= this.MainScroll_Scrolled;
            this.ListViewSpeakers.ItemTapped -= this.ListViewTapped;

            if (this._extension != null)
            {
                await this._extension.Finish();
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.talkViewModel = null;

            this.ListViewSpeakers.HeightRequest = 0;

            var adjust = Device.OS != TargetPlatform.Android ? 1 : -this.ViewModel.SessionMaterialItems.Count + 2;
            this.ListViewSessionMaterial.HeightRequest =
                (this.ViewModel.SessionMaterialItems.Count * this.ListViewSessionMaterial.RowHeight) - adjust;
        }
    }
}

