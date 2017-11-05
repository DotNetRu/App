using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    using System;

    using DotNetRu.DataStore.Audit.Models;

    using FFImageLoading.Forms;

    public partial class SpeakerDetailsPageUWP : BasePage
	{
		public override AppPage PageType => AppPage.Speaker;

        SpeakerDetailsViewModel ViewModel => this.vm ?? (this.vm = this.BindingContext as SpeakerDetailsViewModel);
        SpeakerDetailsViewModel vm;

	    readonly string sessionId;

		public SpeakerDetailsPageUWP(SpeakerModel speakerModel) : this((string)null)
		{
		    this.SpeakerModel = speakerModel;
		}

        public SpeakerDetailsPageUWP(string sessionId)
        {
            this.sessionId = sessionId;
            this.InitializeComponent();

            this.ListViewSessions.ItemSelected += async (sender, e) =>
            {
                var session = this.ListViewSessions.SelectedItem as TalkModel;
                if (session == null)
                    return;

                var sessionDetails = new TalkPage(session);

                await NavigationService.PushAsync(this.Navigation, sessionDetails);

                this.ListViewSessions.SelectedItem = null;
            };

            // HeroImage.Error += HeroImage_Error;
            // HeroImage.Success += HeroImage_Success;
        }

        private void HeroImage_Success(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            // App.Logger.Track($"SpeakerImageLoaded:{e.ImageInformation.FilePath}:{e.LoadingResult}");
        }

        private void HeroImage_Error(object sender, CachedImageEvents.ErrorEventArgs e)
        {
            // App.Logger.Track($"SpeakerImageLoadFailed:{e.Exception}", "Source", HeroImage.Source.ToString());
        }

        private void SpeakerPhoto_SizeChanged(object sender, EventArgs e)
        {
        }

        public SpeakerModel SpeakerModel
        {
            get => this.ViewModel.SpeakerModel;
            set 
			{
			    this.BindingContext = new SpeakerDetailsViewModel(value);
			    this.ItemId = value?.FullName;
			}
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.vm = null;

            this.ListViewFollow.HeightRequest = (this.ViewModel.FollowItems.Count * this.ListViewFollow.RowHeight) - 1;
            this.ListViewSessions.HeightRequest = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewFollow.ItemTapped += this.ListViewTapped;
            this.ListViewSessions.ItemTapped += this.ListViewTapped;

            if (this.ViewModel.Sessions.Count > 0)
                return;

            this.ViewModel.ExecuteLoadSessionsCommandAsync();
            this.ListViewSessions.HeightRequest = (this.ViewModel.Sessions.Count * this.ListViewSessions.RowHeight) - 1;
        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
                return;
            list.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewFollow.ItemTapped -= this.ListViewTapped;
            this.ListViewSessions.ItemTapped -= this.ListViewTapped;
        }
    }
}
