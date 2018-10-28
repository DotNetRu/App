// TODO: It's needed to group meetups not only by month but year too
namespace XamarinEvolve.Clients.Portable.ViewModel
{
    using System;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils.Helpers;

    /// <inheritdoc />
    /// <summary>
    /// The events view model.
    /// </summary>
    public class MeetupsViewModel : ViewModelBase
    {
        /// <summary>
        /// The selected event.
        /// </summary>
        private MeetupModel selectedEvent;

        /// <summary>
        /// The load events command.
        /// </summary>
        private ICommand loadEventsCommand;

        public MeetupsViewModel(INavigation navigation)
            : base(navigation)
        {
            this.Title = "Meetups";
        }

        /// <summary>
        /// The load events command.
        /// </summary>
        public ICommand LoadEventsCommand => this.loadEventsCommand
                                             ?? (this.loadEventsCommand = new Command(
                                                     () => this.ExecuteLoadEventsAsync()));

        /// <summary>
        /// Gets the events.
        /// </summary>
        public ObservableRangeCollection<MeetupModel> Events { get; } =
            new ObservableRangeCollection<MeetupModel>();

        /// <summary>
        /// Gets the events grouped.
        /// </summary>
        public ObservableRangeCollection<Grouping<string, MeetupModel>> EventsGrouped { get; } =
            new ObservableRangeCollection<Grouping<string, MeetupModel>>();

        /// <summary>
        /// Gets or sets the selected event.
        /// </summary>
        public MeetupModel SelectedEvent
        {
            get => this.selectedEvent;
            set
            {
                this.selectedEvent = value;
                this.OnPropertyChanged();
                if (this.selectedEvent == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToEvent, this.selectedEvent);

                this.SelectedEvent = null;
            }
        }

        /// <summary>
        /// The sort events.
        /// </summary>
        public void SortEvents()
        {
            this.EventsGrouped.ReplaceRange(this.Events.GroupByDate());
        }

        /// <summary>
        /// The execute load events async.
        /// </summary>
        public bool ExecuteLoadEventsAsync()
        {
            if (this.IsBusy)
            {
                return false;
            }

            try
            {
                this.IsBusy = true;

                this.Events?.ReplaceRange(MeetupService.GetMeetups());

                this.SortEvents();
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return true;
        }
    }
}