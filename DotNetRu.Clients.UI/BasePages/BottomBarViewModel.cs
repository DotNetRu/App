namespace DotNetRu.Clients.Portable.ViewModel
{
    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.UI.Localization;
    using DotNetRu.Utils.Helpers;

    using Xamarin.Forms;

    public class BottomBarViewModel : ViewModelBase
    {
        public BottomBarViewModel()
        {
            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => this.NotifyViewModel());
        }

        public string NewsTitle => AppResources.News;

        public string SpeakersTitle => AppResources.Speakers;

        public string MeetupsTitle => AppResources.Meetups;

        public string FriendsTitle => AppResources.Friends;

        public string AboutTitle => AppResources.About;

        private void NotifyViewModel()
        {
            this.OnPropertyChanged(nameof(this.NewsTitle));
            this.OnPropertyChanged(nameof(this.SpeakersTitle));
            this.OnPropertyChanged(nameof(this.MeetupsTitle));
            this.OnPropertyChanged(nameof(this.FriendsTitle));
            this.OnPropertyChanged(nameof(this.AboutTitle));
        }
    }
}
