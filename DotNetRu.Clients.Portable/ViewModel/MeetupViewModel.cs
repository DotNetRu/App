namespace XamarinEvolve.Clients.Portable
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
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class MeetupViewModel : ViewModelBase
    {
        public FeaturedEvent Event { get; set; }

        public MeetupViewModel(INavigation navigation, FeaturedEvent featuredEvent = null)
            : base(navigation)
        {
            this.Event = featuredEvent;
            this.NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
        }

        public ObservableRangeCollection<Session> Sessions { get; } = new ObservableRangeCollection<Session>();

        public ObservableRangeCollection<Session> SessionsFiltered { get; } = new ObservableRangeCollection<Session>();

        public ObservableRangeCollection<Grouping<string, Session>> SessionsGrouped { get; } =
            new ObservableRangeCollection<Grouping<string, Session>>();

        public DateTime NextForceRefresh { get; set; }


        #region Properties

        Session selectedSession;

        public Session SelectedSession
        {
            get => this.selectedSession;

            set
            {
                this.selectedSession = value;
                this.OnPropertyChanged();
                if (this.selectedSession == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, this.selectedSession);

                this.SelectedSession = null;
            }
        }

        string filter = string.Empty;

        public string Filter
        {
            get => this.filter;

            set
            {
                if (this.SetProperty(ref this.filter, value)) this.ExecuteFilterSessionsAsync();

            }
        }

        #endregion

        #region Filtering and Sorting


        void SortSessions()
        {
            this.SessionsGrouped.ReplaceRange(this.SessionsFiltered.FilterAndGroupByDate());
        }

        bool noSessionsFound;

        public bool NoSessionsFound
        {
            get => this.noSessionsFound;

            set => this.SetProperty(ref this.noSessionsFound, value);
        }

        string noSessionsFoundMessage;

        public string NoSessionsFoundMessage
        {
            get => this.noSessionsFoundMessage;

            set => this.SetProperty(ref this.noSessionsFoundMessage, value);
        }

        #endregion


        #region Commands

        ICommand forceRefreshCommand;

        public ICommand ForceRefreshCommand => this.forceRefreshCommand
                                               ?? (this.forceRefreshCommand = new Command(
                                                       async () => await this.ExecuteForceRefreshCommandAsync()));

        async Task ExecuteForceRefreshCommandAsync()
        {
            await this.ExecuteLoadSessionsAsync(true);
        }

        ICommand filterSessionsCommand;

        public ICommand FilterSessionsCommand => this.filterSessionsCommand
                                                 ?? (this.filterSessionsCommand = new Command(
                                                         async () => await this.ExecuteFilterSessionsAsync()));

        async Task ExecuteFilterSessionsAsync()
        {
            this.IsBusy = true;
            this.NoSessionsFound = false;

            // Abort the current command if the user is typing fast
            if (!string.IsNullOrEmpty(this.Filter))
            {
                var query = this.Filter;
                await Task.Delay(250);
                if (query != this.Filter) return;
            }

            this.SessionsFiltered.ReplaceRange(this.Sessions.Search(this.Filter));
            this.SortSessions();

            if (this.SessionsGrouped.Count == 0)
            {
                this.NoSessionsFoundMessage = "No Sessions Found";
                this.NoSessionsFound = true;
            }
            else
            {
                this.NoSessionsFound = false;
            }

            this.IsBusy = false;
        }

        ICommand loadSessionsCommand;

        public ICommand LoadSessionsCommand => this.loadSessionsCommand
                                               ?? (this.loadSessionsCommand = new Command<bool>(
                                                       async (f) => await this.ExecuteLoadSessionsAsync()));

        List<Session> TalkToSessionConverter(IEnumerable<Talk> talks)
        {
            return talks.Select(
                talk => new Session
                            {
                                Title = talk.Title,
                                Abstract = talk.Description,
                                PresentationUrl = talk.SlidesUrl,
                                VideoUrl = talk.VideoUrl,
                                CodeUrl = talk.CodeUrl,
                                ShortTitle = talk.Title,
                                StartTime =
                                    this.Event.StartTime?.ToLocalTime().AddHours(15), // TODO: It's a zaglushka
                                EndTime = this.Event.StartTime?.ToLocalTime().AddHours(18),
                                Speakers = SpeakerLoaderService.Speakers
                                    .Where(s => talk.SpeakerIds.Any(s1 => s1 == s.Id)).ToList()
                            }).ToList();
        }

        List<Session> GetSessions()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.talks.xml");
            IEnumerable<Talk> sessions;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Talks", IsNullable = false };
                var serializer = new XmlSerializer(typeof(List<Talk>), xRoot);
                sessions = ((List<Talk>)serializer.Deserialize(reader)).Where(
                    t => this.Event.EventTalksIds.Any(t1 => t1 == t.Id));
            }

            return this.TalkToSessionConverter(sessions);
        }

        async Task<bool> ExecuteLoadSessionsAsync(bool force = false)
        {
            if (this.IsBusy) return false;

            try
            {
                this.NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                this.IsBusy = true;
                this.NoSessionsFound = false;
                this.Filter = string.Empty;

                this.Sessions.ReplaceRange(this.GetSessions() /*await StoreManager.SessionStore.GetItemsAsync(force)*/);

                this.SessionsFiltered.ReplaceRange(this.Sessions);
                this.SortSessions();

                if (this.SessionsGrouped.Count == 0)
                {
                    this.NoSessionsFoundMessage = "No Sessions Found";
                    this.NoSessionsFound = true;
                }
                else
                {
                    this.NoSessionsFound = false;
                }

                if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows
                    && FeatureFlags.AppLinksEnabled)
                {
                    foreach (var session in this.Sessions)
                    {
                        try
                        {
                            // TODO uncomment

                            // data migration: older applinks are removed so the index is rebuilt again
                            // Application.Current.AppLinks.DeregisterLink(
                            // new Uri(
                            // $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory}/{session.Id}"));

                            // Application.Current.AppLinks.RegisterLink(session.GetAppLink());
                        }
                        catch (Exception applinkException)
                        {
                            // don't crash the app
                            this.Logger.Report(applinkException, "AppLinks.RegisterLink", session.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadSessionsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return true;
        }

        #endregion
    }
}