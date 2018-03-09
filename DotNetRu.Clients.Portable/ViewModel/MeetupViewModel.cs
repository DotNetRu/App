namespace DotNetRu.Clients.Portable.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using Xamarin.Forms;

    public class MeetupViewModel : ViewModelBase
    {
        private TalkModel selectedTalkModel;

        public MeetupViewModel(INavigation navigation, MeetupModel meetupModel = null, VenueModel venueModel = null)
            : base(navigation)
        {
            this.MeetupModel = meetupModel;
            this.VenueModel = venueModel;

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.OnPropertyChanged(nameof(this.MeetupTime)));

            this.TapVenueCommand = new Command(this.OnVenueTapped);
        }

        public IEnumerable<TalkModel> Talks => this.MeetupModel.Talks;                

        public MeetupModel MeetupModel { get; set; }

        public VenueModel VenueModel { get; set; }

        public string MeetupTime => this.MeetupModel.StartTime?.ToString("D");

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

        public bool NoSessionsFound => !this.MeetupModel.Talks.Any();

        public ICommand TapVenueCommand { get; set; }

        public void OnVenueTapped()
        {
            this.LaunchBrowserCommand.Execute(this.VenueModel.MapUrl);
        }
    }
}