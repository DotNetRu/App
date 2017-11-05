using System;
using System.Threading.Tasks;
using System.Windows.Input;

using FormsToolkit;

using MvvmHelpers;

using Xamarin.Forms;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.Models;

    using XamarinEvolve.Utils.Helpers;

	public class EventDetailsViewModel : ViewModelBase
    {
        public MeetupModel Event { get; set; }

        public ObservableRangeCollection<FriendModel> Sponsors { get; set; }

        public EventDetailsViewModel(INavigation navigation, MeetupModel e) : base(navigation)
        {
            this.Event = e;
            this.Sponsors = new ObservableRangeCollection<FriendModel>();
            if (e.FriendModel != null) this.Sponsors.Add(e.FriendModel);
        }

        bool isReminderSet;
        public bool IsReminderSet
        {
            get => this.isReminderSet;
            set => this.SetProperty(ref this.isReminderSet, value);
        }

        ICommand  loadEventDetailsCommand;
        public ICommand LoadEventDetailsCommand => this.loadEventDetailsCommand ?? (this.loadEventDetailsCommand = new Command(async () => await this.ExecuteLoadEventDetailsCommandAsync())); 

        async Task ExecuteLoadEventDetailsCommandAsync()
        {

            if(this.IsBusy)
                return;

            try 
            {
                this.IsBusy = true;
                this.IsReminderSet = await ReminderService.HasReminderAsync("event_" + this.Event.Id);
            } 
            catch (Exception ex) 
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadEventDetailsCommandAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        ICommand  reminderCommand;
        public ICommand ReminderCommand => this.reminderCommand ?? (this.reminderCommand = new Command(async () => await this.ExecuteReminderCommandAsync())); 


        async Task ExecuteReminderCommandAsync()
        {
            if(!this.IsReminderSet)
            {
                var result = await ReminderService.AddReminderAsync("event_" + this.Event.Id, 
                    new Plugin.Calendars.Abstractions.CalendarEvent
                    {
                        Description = this.Event.Description,
                        Location = this.Event.VenueID,
                        AllDay = this.Event.IsAllDay,
                        Name = this.Event.Title,
                        Start = this.Event.StartTime.Value,
                        End = this.Event.EndTime.Value
                    });


                if(!result)
                    return;

                this.Logger.Track(EvolveLoggerKeys.ReminderAdded, "Title", this.Event.Title);
                this.IsReminderSet = true;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync("event_" + this.Event.Id);
                if(!result)
                    return;
                this.Logger.Track(EvolveLoggerKeys.ReminderRemoved, "Title", this.Event.Title);
                this.IsReminderSet = false;
            }

        }

        FriendModel selectedFriendModel;
        public FriendModel SelectedFriendModel
        {
            get => this.selectedFriendModel;
            set
            {
                this.selectedFriendModel = value;
                this.OnPropertyChanged();
                if (this.selectedFriendModel == null)
                    return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSponsor, this.selectedFriendModel);

                this.SelectedFriendModel = null;
            }
        }

    }
}


