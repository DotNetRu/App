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


// TODO: It's needed to group meetups not only by month but year too
namespace XamarinEvolve.Clients.Portable.ViewModel
{
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
            Title = "Meetups";
        }

        /// <summary>
        /// The force refresh command.
        /// </summary>
        public ICommand ForceRefreshCommand => forceRefreshCommand
                                               ?? (forceRefreshCommand = new Command(
                                                       async () => await ExecuteForceRefreshCommandAsync()));

        /// <summary>
        /// The load events command.
        /// </summary>
        public ICommand LoadEventsCommand => loadEventsCommand
                                             ?? (loadEventsCommand = new Command<bool>(
                                                     async f => await ExecuteLoadEventsAsync()));

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
            get => selectedEvent;
            set
            {
                selectedEvent = value;
                OnPropertyChanged();
                if (selectedEvent == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToEvent, selectedEvent);

                SelectedEvent = null;
            }
        }

        /// <summary>
        /// The sort events.
        /// </summary>
        public void SortEvents()
        {
            EventsGrouped.ReplaceRange(Events.GroupByDate());
        }

        /// <summary>
        /// The execute force refresh command async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteForceRefreshCommandAsync()
        {
            await ExecuteLoadEventsAsync(true);
        }

        private List<FeaturedEvent> MeetupsToFeaturedEvents(IEnumerable<Meetup> meetups)
        {
            return meetups.Select(meetup => new FeaturedEvent
            {
                Description = meetup.Name,
                IsAllDay = true,
                Title = meetup.Name,
                StartTime = meetup.Date,
                EndTime = meetup.Date,
                LocationName = meetup.VenueId
            }).ToList();
        }

        private List<FeaturedEvent> GetEvents()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.meetups.xml");
            List<Meetup> meetups;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute
                {
                    ElementName = "Meetups",
                    IsNullable = false
                };
                var serializer = new XmlSerializer(typeof(List<Meetup>), xRoot);
                meetups = (List<Meetup>) serializer.Deserialize(reader);
            }
            return MeetupsToFeaturedEvents(meetups);
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
            if (IsBusy)
            {
                return false;
            }

            try
            {
                IsBusy = true;

                // TODO: update data when we'll have finally managed to get them directly from github
                Events?.ReplaceRange(GetEvents());
                //Events.ReplaceRange(await StoreManager.EventStore.GetItemsAsync(force));

                Title = "Meetups (" + Events?.Count(
                                 e => e.StartTime.HasValue && e.StartTime.Value.ToUniversalTime() > DateTime.UtcNow)
                             + ")";

                SortEvents();
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }
    }
}
