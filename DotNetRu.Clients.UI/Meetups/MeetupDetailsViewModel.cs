using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.Clients.UI.Pages.Sessions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;
using MvvmHelpers;
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
                sender => OnPropertyChanged(nameof(MeetupDate)));

            MeetupDetailsPageItems.ReplaceRange(GetMeetupDetailsPageItems());
        }

        private IEnumerable<Grouping<string, MeetupDetailsPageItem>> GetMeetupDetailsPageItems()
        {
            var pageItems = new List<Grouping<string, MeetupDetailsPageItem>>();

            var sessions = MeetupModel.Sessions.ToList();
            pageItems.Add(new Grouping<string, MeetupDetailsPageItem>("Sessions", sessions.Select(x => new MeetupDetailsPageItem
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

            return pageItems;
        }

        public IReadOnlyList<SessionModel> Sessions => MeetupModel.Sessions.ToList();

        public IReadOnlyList<FriendModel> Friends => MeetupModel.Friends.ToList();

        public MeetupModel MeetupModel { get; set; }

        public VenueModel VenueModel => MeetupModel.Venue;

        public string MeetupDate => MeetupModel.StartTime?.ToString("D");

        public string MeetupTime => $"{Sessions.First().StartTime.LocalDateTime.ToShortTimeString()} — {Sessions.Last().EndTime.LocalDateTime.ToShortTimeString()}";

        public ICommand SelectedItemCommand => new Command<MeetupDetailsPageItem>(async item => await HandleSelection(item));

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
            }
        }

        public ICommand ReminderCommand => new Command(() => this.ExecuteReminderCommand());

        private void ExecuteReminderCommand()
        {
            //var calendarEvent = new CalendarEvent
            // //{		             {
            // //    var result = await ReminderService.AddReminderAsync(		                 AllDay = false,
            // //                     this.TalkModel.Id,		                 Description = this.TalkModel.Abstract,
            // //                     new Plugin.Calendars.Abstractions.CalendarEvent		                 Location = this.TalkModel.Sessions.First().Meetup.Venue.Address,
            // //                         {		                 Name = this.TalkModel.Title,
            // //                             AllDay = false,		                 Start = this.TalkModel.Sessions.First().StartTime.LocalDateTime,
            // //                             Description =		                 End = this.TalkModel.Sessions.First().EndTime.LocalDateTime
            // //                                 this.TalkModel.Abstract,		             };
            // //                             Location =		             if (this.IsReminderSet)
            // //                                 this.TalkModel.Room?.Name		             {
            // //                                 ?? string.Empty,		                 var result = await ReminderService.DeleteSessionAsync(calendarEvent);
            // //                             Name = this.TalkModel.Title,		
            // //                             Start = this.TalkModel.StartTime		                 if (!result)
            // //                                 .Value,		                 {
            // //                             End = this.TalkModel.EndTime.Value		                     return;
            // //                         });		                 }



            //MessagingUtils.SendToast(this.Resources["RemovedFromCalendar"]);
            ////    if (!result)		                 this.Logger.Track(DotNetRuLoggerKeys.ReminderRemoved, "Title", this.TalkModel.Title);
            ////    {		                 this.IsReminderSet = false;
            ////        return;		             }
            ////    }		             else

            //{
            //    //    this.Logger.Track(DotNetRuLoggerKeys.ReminderAdded, "Title", this.TalkModel.Title);		                 var result = await ReminderService.AddSessionAsync(calendarEvent);
            //    //    this.IsReminderSet = true;		
            //    //}		                 if (!result)
            //    //else		                 {
            //    //{		                     return;
            //    //    var result = await ReminderService.RemoveReminderAsync(this.TalkModel.Id);		                 }
            //    //    if (!result)		
            //    //    {		                 MessagingUtils.SendToast(this.Resources["AddedToCalendar"]);
            //    //        return;		                 this.Logger.Track(DotNetRuLoggerKeys.ReminderAdded, "Title", this.TalkModel.Title);
            //    //    }		                 this.IsReminderSet = true;

            //}
        }
    }
}
