using DotNetRu.Clients.Portable.Helpers;
using Xamarin.Forms;
using XamarinEvolve.Utils.Helpers;

namespace DotNetRu.Clients.Portable.ViewModel
{
    public class BottomBarViewModel : ViewModelBase
    {
        public BottomBarViewModel()
        {
            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => this.NotifyViewModel());
        }

        public string NewsTitle => this.Resources["News"];

        public string SpeakersTitle => this.Resources["Speakers"];

        public string MeetupsTitle => this.Resources["Meetups"];

        public string FriendsTitle => this.Resources["Friends"];

        public string AboutTitle => this.Resources["About"];

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
