namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.Portable.ApplicationResources;

    public partial class SpeakerDetailsPage
    {
        public override AppPage PageType => AppPage.Speaker;

        private readonly IPlatformSpecificExtension<SpeakerModel> _extension;

        public SpeakerDetailsViewModel SpeakerDetailsViewModel => this.speakerDetailsViewModel
                                             ?? (this.speakerDetailsViewModel =
                                                     this.BindingContext as SpeakerDetailsViewModel);

        private SpeakerDetailsViewModel speakerDetailsViewModel;

        public SpeakerDetailsPage(SpeakerModel speakerModel)
            : this()
        {
            this.SpeakerModel = speakerModel;
        }

        public SpeakerDetailsPage()
        {
            this.InitializeComponent();
            this.MainScroll.ParallaxView = this.HeaderView;
            this._extension = DependencyService.Get<IPlatformSpecificExtension<SpeakerModel>>();

            this.ListViewSessions.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewSessions.SelectedItem is TalkModel session))
                    {
                        return;
                    }

                    var sessionDetails = new TalkPage(session);

                    await NavigationService.PushAsync(this.Navigation, sessionDetails);

                    this.ListViewSessions.SelectedItem = null;
                };

            if (Device.Idiom != TargetIdiom.Phone)
            {
                this.Row1Header.Height = this.Row1Content.Height = 350;
            }
        }

        public SpeakerModel SpeakerModel
        {
            get => this.SpeakerDetailsViewModel.SpeakerModel;
            set
            {
                this.BindingContext = new SpeakerDetailsViewModel(value);
                this.ItemId = value?.FullName;
            }
        }

        void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            this.Title = e.ScrollY > (this.MainStack.Height - this.SpeakerTitle.Height) ? this.SpeakerModel.FirstName : AppResources.SpeakerInfo;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // MainStack.HeightRequest = HeaderView.Height;
            this.MainScroll.Parallax();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.speakerDetailsViewModel = null;

            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.SpeakerDetailsViewModel.FollowItems.Count + 2;
            this.ListViewFollow.HeightRequest =
                (this.SpeakerDetailsViewModel.FollowItems.Count * this.ListViewFollow.RowHeight) - adjust;
            this.ListViewSessions.HeightRequest = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            this.MainScroll.Scrolled += this.MainScroll_Scrolled;
            this.ListViewFollow.ItemTapped += this.ListViewTapped;
            this.ListViewSessions.ItemTapped += this.ListViewTapped;

            this.MainScroll.Parallax();

            if (this.SpeakerDetailsViewModel.Talks?.Count > 0)
            {
                return;
            }

            this.SpeakerDetailsViewModel.ExecuteLoadTalksCommand();
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.SpeakerDetailsViewModel.Talks.Count + 2;
            this.ListViewSessions.HeightRequest =
                (this.SpeakerDetailsViewModel.Talks.Count * this.ListViewSessions.RowHeight) - adjust;

            if (this._extension != null)
            {
                await this._extension.Execute(this.SpeakerDetailsViewModel.SpeakerModel);
            }
        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewFollow.ItemTapped -= this.ListViewTapped;
            this.ListViewSessions.ItemTapped -= this.ListViewTapped;
            this.MainScroll.Scrolled -= this.MainScroll_Scrolled;

            if (this._extension != null)
            {
                await this._extension.Finish();
            }
        }
    }
}
