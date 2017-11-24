using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinEvolve.Clients.UI;
using XamarinEvolve.Utils.Helpers;

namespace XamarinEvolve.Clients.Portable.ViewModel
{
    public class BottomBarViewModel : ViewModelBase
    {
        public BottomBarViewModel()
        {
            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => NotifyVmProperties());

        }

        private void NotifyVmProperties()
        {
            OnPropertyChanged(nameof(NewsTitle));
            OnPropertyChanged(nameof(SpeakersTitle));
            OnPropertyChanged(nameof(MeetupsTitle));
            OnPropertyChanged(nameof(FriendsTitle));
            OnPropertyChanged(nameof(AboutTitle));

        }

        public string NewsTitle => Resources["News"];
        public string SpeakersTitle => Resources["Speakers"];
        public string MeetupsTitle => Resources["Meetups"];
        public string FriendsTitle => Resources["Friends"];
        public string AboutTitle => Resources["About"];
    }
}
