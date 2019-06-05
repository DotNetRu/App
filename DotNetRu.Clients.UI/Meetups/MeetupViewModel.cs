using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.Clients.UI.Pages.Sessions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;

using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Meetups
{
    public class MeetupViewModel : ViewModelBase
    {
        public MeetupViewModel(INavigation navigation, MeetupModel meetupModel = null)
            : base(navigation)
        {
            MeetupModel = meetupModel;

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => OnPropertyChanged(nameof(MeetupDate)));
        }

        public IReadOnlyList<SessionModel> Sessions => MeetupModel.Sessions.ToList();

        public IReadOnlyList<FriendModel> Friends => MeetupModel.Friends.ToList();

        public MeetupModel MeetupModel { get; set; }

        public VenueModel VenueModel => MeetupModel.Venue;

        public string MeetupDate => MeetupModel.StartTime?.ToString("D");

        public string MeetupTime => $"{Sessions.First().StartTime.LocalDateTime.ToShortTimeString()} — {Sessions.Last().EndTime.LocalDateTime.ToShortTimeString()}";

        public bool NoSessionsFound => !MeetupModel.Sessions.Any();

        public ICommand TapVenueCommand => new Command(() => LaunchBrowserCommand.Execute(VenueModel.MapUrl));

        public ICommand SessionSelectedCommand => new Command<TalkModel>(async session =>
        {
            App.Logger.TrackPage(AppPage.Talk.ToString(), session.Title);
            await NavigationService.PushAsync(Navigation, new TalkPage(session));
        });

        public ICommand FriendSelectedCommand => new Command<FriendModel>(async friend =>
        {
            var sponsorDetails = new FriendDetailsPage
            {
                FriendModel = friend
            };

            App.Logger.TrackPage(AppPage.Friend.ToString(), friend.Name);
            await NavigationService.PushAsync(Navigation, sponsorDetails);
        });
    }
}
