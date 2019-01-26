using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;

using FormsToolkit;

using Xamarin.Forms;

namespace DotNetRu.Clients.Portable.ViewModel
{    
    public class MeetupViewModel : ViewModelBase
    {
        private TalkModel selectedTalkModel;

        public MeetupViewModel(INavigation navigation, MeetupModel meetupModel = null)
            : base(navigation)
        {
            this.MeetupModel = meetupModel;

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.OnPropertyChanged(nameof(this.MeetupDate)));

            this.TapVenueCommand = new Command(this.OnVenueTapped);
        }

        public IReadOnlyList<SessionModel> Sessions => this.MeetupModel.Sessions.ToList();

        public IReadOnlyList<FriendModel> Friends => this.MeetupModel.Friends.ToList();

        public MeetupModel MeetupModel { get; set; }

        public VenueModel VenueModel => this.MeetupModel.Venue;

        public string MeetupDate => this.MeetupModel.StartTime?.ToString("D");

        public string MeetupTime => $"{this.Sessions.First().StartTime.LocalDateTime.ToShortTimeString()} — {this.Sessions.Last().EndTime.LocalDateTime.ToShortTimeString()}";

        public TalkModel SelectedTalkModel
        {
            get => this.selectedTalkModel;

            set
            {
                this.selectedTalkModel = value;
                this.OnPropertyChanged();
                if (this.selectedTalkModel == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, this.selectedTalkModel);

                this.SelectedTalkModel = null;
            }
        }

        public bool NoSessionsFound => !this.MeetupModel.Sessions.Any();

        public ICommand TapVenueCommand { get; set; }

        public void OnVenueTapped()
        {
            this.LaunchBrowserCommand.Execute(this.VenueModel.MapUrl);
        }
    }
}