using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.News;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.Clients.UI.Pages.Sessions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;
using DotNetRu.Utils.Interfaces;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Plugin.Calendars.Abstractions;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Meetups
{
    public class MeetupDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Grouping<string, MeetupDetailsPageItem>> MeetupDetailsPageItems { get; } =
            new ObservableRangeCollection<Grouping<string, MeetupDetailsPageItem>>();

        public MeetupDetailsViewModel(INavigation navigation, MeetupModel meetupModel = null)
            : base(navigation)
        {
            MeetupModel = meetupModel;

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender =>
                {
                    MeetupDetailsPageItems.ReplaceRange(GetMeetupDetailsPageItems());
                }
                );

            UpdateRemainderStatusCommand.Execute(parameter: null);

            MeetupDetailsPageItems.ReplaceRange(GetMeetupDetailsPageItems());
        }

        private IEnumerable<Grouping<string, MeetupDetailsPageItem>> GetMeetupDetailsPageItems()
        {
            var pageItems = new List<Grouping<string, MeetupDetailsPageItem>>();

            var sessions = MeetupModel.Sessions.ToList();
            pageItems.Add(new Grouping<string, MeetupDetailsPageItem>("Talks", sessions.Select(x => new MeetupDetailsPageItem
            {
                Session = x,
                ItemType = MeetupDetailsItemType.Session
            })));

            pageItems.Add(new Grouping<string, MeetupDetailsPageItem>("Venue", new MeetupDetailsPageItem[] {
                new MeetupDetailsPageItem
                {
                    ItemType = MeetupDetailsItemType.Venue
                }
            }));

            var friends = MeetupModel.Friends.ToList();
            pageItems.Add(new Grouping<string, MeetupDetailsPageItem>("Friends", friends.Select(x => new MeetupDetailsPageItem
            {
                Friend = x,
                ItemType = MeetupDetailsItemType.Friend
            })));

            pageItems.Add(new Grouping<string, MeetupDetailsPageItem>("Calendar", new MeetupDetailsPageItem[] {
                new MeetupDetailsPageItem
                {
                    ItemType = MeetupDetailsItemType.Calendar
                }
            }));

            return pageItems;
        }

        public IReadOnlyList<SessionModel> Sessions => MeetupModel.Sessions.ToList();

        public IReadOnlyList<FriendModel> Friends => MeetupModel.Friends.ToList();

        public MeetupModel MeetupModel { get; set; }

        public VenueModel VenueModel => MeetupModel.Venue;

        public string MeetupDate => MeetupModel.StartTime?.ToString("D");

        public string MeetupTime => $"{Sessions.First().StartTime.LocalDateTime.ToShortTimeString()} — {Sessions.Last().EndTime.LocalDateTime.ToShortTimeString()}";

        public ICommand UpdateRemainderStatusCommand => new Command(async () => await ExecuteLoadEventDetailsCommandAsync());

        private async Task ExecuteLoadEventDetailsCommandAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                IsReminderSet = await CalendarService.HasReminderAsync(MeetupModel.Id);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    public ICommand SelectedItemCommand => new Command<MeetupDetailsPageItem>(async item => await HandleSelection(item));

        private bool isReminderSet;
        public bool IsReminderSet
        {
            get
            {
                return isReminderSet;
            }
            set
            {
                isReminderSet = value;
                OnPropertyChanged(nameof(IsReminderSet));
            }
        }

        private async Task HandleSelection(MeetupDetailsPageItem item)
        {
            switch (item.ItemType)
            {
                case MeetupDetailsItemType.Friend:
                    var sponsorDetails = new FriendDetailsPage
                    {
                        FriendModel = item.Friend
                    };

                    App.Logger.TrackPage(AppPage.Friend.ToString(), item.Friend.Name);
                    await NavigationService.PushAsync(Navigation, sponsorDetails);
                    break;
                case MeetupDetailsItemType.Venue:
                    await ExecuteLaunchBrowserAsync(new Uri(VenueModel.MapUrl));
                    break;
                case MeetupDetailsItemType.Session:
                    App.Logger.TrackPage(AppPage.Talk.ToString(), item.Session.Talk.Title);
                    await NavigationService.PushAsync(Navigation, new TalkPage(item.Session.Talk));
                    break;
                case MeetupDetailsItemType.Calendar:
                    var toaster = DependencyService.Get<IToast>();

                    if (this.IsReminderSet)
                    {
                        var result = await CalendarService.RemoveCalendarEventAsync(MeetupModel.Id);

                        if (!result)
                        {
                            toaster.SendToast(this.Resources["ErrorOccured"]);
                            return;
                        }

                        toaster.SendToast(this.Resources["RemovedFromCalendar"]);
                        this.Logger.Track(DotNetRuLoggerKeys.ReminderRemoved, "Title", this.MeetupModel.Title);
                        this.IsReminderSet = false;
                    }
                    else
                    {
                        var calendarEvent = new CalendarEvent
                        {
                            AllDay = false,
                            Description = this.MeetupModel.Title,
                            Location = this.Sessions.First().Meetup.Venue.Address,
                            Name = this.MeetupModel.Title,
                            Start = this.Sessions.First().StartTime.LocalDateTime,
                            End = this.Sessions.Last().EndTime.LocalDateTime,
                            Reminders = new[] {
                                new CalendarEventReminder                                {                                    Method = CalendarReminderMethod.Default,                                    TimeBefore = TimeSpan.FromMinutes(60)                                }
                            }   
                        };

                        var result = await CalendarService.AddCalendarEventAsync(MeetupModel.Id, calendarEvent);
                        if (!result)
                        {
                            toaster.SendToast(this.Resources["ErrorOccured"]);
                            return;
                        }

                        toaster.SendToast(this.Resources["AddedToCalendar"]);

                        this.Logger.Track(DotNetRuLoggerKeys.ReminderAdded, "Title", this.MeetupModel.Title);
                        this.IsReminderSet = true;
                    }
                    break;
            }
        }
    }
}
