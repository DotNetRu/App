using System.Diagnostics;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.Clients.UI
{
    public partial class SpeakerDetailsPageUWP : BasePage
	{
		public override AppPage PageType => AppPage.Speaker;

        SpeakerDetailsViewModel ViewModel => vm ?? (vm = BindingContext as SpeakerDetailsViewModel);
        SpeakerDetailsViewModel vm;
        string sessionId;

		public SpeakerDetailsPageUWP(Speaker speaker) : this((string)null)
		{
			Speaker = speaker;
		}

        public SpeakerDetailsPageUWP(string sessionId)
        {
            this.sessionId = sessionId;
            InitializeComponent();

            ListViewSessions.ItemSelected += async (sender, e) =>
            {
                var session = ListViewSessions.SelectedItem as Session;
                if (session == null)
                    return;

                var sessionDetails = new SessionDetailsPage(session);

                await NavigationService.PushAsync(Navigation, sessionDetails);

                ListViewSessions.SelectedItem = null;
            };

            //HeroImage.Error += HeroImage_Error;
            //HeroImage.Success += HeroImage_Success;
        }

        private void HeroImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            //App.Logger.Track($"SpeakerImageLoaded:{e.ImageInformation.FilePath}:{e.LoadingResult}");
        }

        private void HeroImage_Error(object sender, FFImageLoading.Forms.CachedImageEvents.ErrorEventArgs e)
        {
            //App.Logger.Track($"SpeakerImageLoadFailed:{e.Exception}", "Source", HeroImage.Source.ToString());
        }

        private void SpeakerPhoto_SizeChanged(object sender, System.EventArgs e)
        {
        }

        public Speaker Speaker
        {
            get { return ViewModel.Speaker; }
            set 
			{
				BindingContext = new SpeakerDetailsViewModel(value, sessionId);
				ItemId = value?.FullName;
			}
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            ListViewFollow.HeightRequest = (ViewModel.FollowItems.Count * ListViewFollow.RowHeight) - 1;
            ListViewSessions.HeightRequest = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ListViewFollow.ItemTapped += ListViewTapped;
            ListViewSessions.ItemTapped += ListViewTapped;

            if (ViewModel.Sessions.Count > 0)
                return;

            await ViewModel.ExecuteLoadSessionsCommandAsync();
            ListViewSessions.HeightRequest = (ViewModel.Sessions.Count * ListViewSessions.RowHeight) - 1;
        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ListViewFollow.ItemTapped -= ListViewTapped;
            ListViewSessions.ItemTapped -= ListViewTapped;
        }
    }
}
