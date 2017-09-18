namespace XamarinEvolve.Clients.Portable.ViewModel
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils.Helpers;

    /// <summary>
    /// The events view model.
    /// </summary>
    public class EventsViewModel : ViewModelBase
    {
        /// <summary>
        /// The selected event.
        /// </summary>
        private FeaturedEvent selectedEvent;

        /// <summary>
        /// The force refresh command.
        /// </summary>
        private ICommand forceRefreshCommand;

        /// <summary>
        /// The load events command.
        /// </summary>
        private ICommand loadEventsCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsViewModel"/> class.
        /// </summary>
        /// <param name="navigation">
        /// The navigation.
        /// </param>
        public EventsViewModel(INavigation navigation)
            : base(navigation)
        {
            this.Title = "Meetups";
        }

        /// <summary>
        /// The force refresh command.
        /// </summary>
        public ICommand ForceRefreshCommand => this.forceRefreshCommand
                                               ?? (this.forceRefreshCommand = new Command(
                                                       async () => await this.ExecuteForceRefreshCommandAsync()));

        /// <summary>
        /// The load events command.
        /// </summary>
        public ICommand LoadEventsCommand => this.loadEventsCommand
                                             ?? (this.loadEventsCommand = new Command<bool>(
                                                     async (f) => await this.ExecuteLoadEventsAsync()));

        /// <summary>
        /// Gets the events.
        /// </summary>
        public ObservableRangeCollection<FeaturedEvent> Events { get; } =
            new ObservableRangeCollection<FeaturedEvent>();

        /// <summary>
        /// Gets the events grouped.
        /// </summary>
        public ObservableRangeCollection<Grouping<string, FeaturedEvent>> EventsGrouped { get; } =
            new ObservableRangeCollection<Grouping<string, FeaturedEvent>>();

        /// <summary>
        /// Gets or sets the selected event.
        /// </summary>
        public FeaturedEvent SelectedEvent
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
        /// The execute force refresh command async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteForceRefreshCommandAsync()
        {
            await this.ExecuteLoadEventsAsync(true);
        }

        /// <summary>
        /// The execute load events async.
        /// </summary>
        /// <param name="force">
        /// The force.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> ExecuteLoadEventsAsync(bool force = false)
        {
            if (this.IsBusy)
            {
                return false;
            }

            try
            {
                this.IsBusy = true;

                this.Events.ReplaceRange(await this.StoreManager.EventStore.GetItemsAsync(force));

                this.Title = "Meetups (" + this.Events.Count(
                                 e => e.StartTime.HasValue && e.StartTime.Value.ToUniversalTime() > DateTime.UtcNow)
                             + ")";

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
