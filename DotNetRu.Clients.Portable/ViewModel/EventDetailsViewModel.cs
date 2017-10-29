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
        public FeaturedEvent Event { get; set; }

        public ObservableRangeCollection<Sponsor> Sponsors { get; set; }

        public EventDetailsViewModel(INavigation navigation, FeaturedEvent e) : base(navigation)
        {
            this.Event = e;
            this.Sponsors = new ObservableRangeCollection<Sponsor>();
            if (e.Sponsor != null) this.Sponsors.Add(e.Sponsor);
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
                        Location = this.Event.LocationName,
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

        Sponsor selectedSponsor;
        public Sponsor SelectedSponsor
        {
            get => this.selectedSponsor;
            set
            {
                this.selectedSponsor = value;
                this.OnPropertyChanged();
                if (this.selectedSponsor == null)
                    return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSponsor, this.selectedSponsor);

                this.SelectedSponsor = null;
            }
        }

    }
}


