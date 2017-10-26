// TODO: It's needed to group meetups not only by month but year too
namespace XamarinEvolve.Clients.Portable.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using DotNetRu.DataStore.Audit.Models;

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
                                                     async f => await this.ExecuteLoadEventsAsync()));

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

                // TODO: update data when we'll have finally managed to get them directly from github
                this.Events?.ReplaceRange(this.GetEvents());

                // Events.ReplaceRange(await StoreManager.EventStore.GetItemsAsync(force));
                this.Title = "Meetups"; // (" + this.Events?.Count(e => e.StartTime.HasValue) + ")";

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

        private List<FeaturedEvent> MeetupsToFeaturedEvents(IEnumerable<Meetup> meetups)
        {
            return meetups.Select(
                meetup => new FeaturedEvent
                              {
                                  Description = meetup.Name,
                                  IsAllDay = true,
                                  Title = meetup.Name,
                                  StartTime = meetup.Date,
                                  EndTime = meetup.Date,
                                  LocationName = meetup.VenueId,
                                  EventTalksIds = meetup.TalkIds,
                              }).ToList();
        }

        private List<FeaturedEvent> GetEvents()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.meetups.xml");
            List<Meetup> meetups;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Meetups", IsNullable = false };
                var serializer = new XmlSerializer(typeof(List<Meetup>), xRoot);
                meetups = (List<Meetup>)serializer.Deserialize(reader);
            }

            return this.MeetupsToFeaturedEvents(meetups);
        }
    }
}
