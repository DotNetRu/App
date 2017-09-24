namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Helpers;

    public class SessionsViewModel : ViewModelBase
    {
        public SessionsViewModel(INavigation navigation)
            : base(navigation)
        {
            NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
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
            get
            {
                return selectedSession;
            }
            set
            {
                selectedSession = value;
                OnPropertyChanged();
                if (selectedSession == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, selectedSession);

                SelectedSession = null;
            }
        }

        string filter = string.Empty;

        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (SetProperty(ref filter, value)) ExecuteFilterSessionsAsync();

            }
        }

        #endregion

        #region Filtering and Sorting


        void SortSessions()
        {
            SessionsGrouped.ReplaceRange(SessionsFiltered.FilterAndGroupByDate());
        }

        bool noSessionsFound;

        public bool NoSessionsFound
        {
            get
            {
                return noSessionsFound;
            }
            set
            {
                SetProperty(ref noSessionsFound, value);
            }
        }

        string noSessionsFoundMessage;

        public string NoSessionsFoundMessage
        {
            get
            {
                return noSessionsFoundMessage;
            }
            set
            {
                SetProperty(ref noSessionsFoundMessage, value);
            }
        }

        #endregion


        #region Commands

        ICommand forceRefreshCommand;

        public ICommand ForceRefreshCommand => forceRefreshCommand
                                               ?? (forceRefreshCommand = new Command(
                                                       async () => await ExecuteForceRefreshCommandAsync()));

        async Task ExecuteForceRefreshCommandAsync()
        {
            await ExecuteLoadSessionsAsync(true);
        }

        ICommand filterSessionsCommand;

        public ICommand FilterSessionsCommand => filterSessionsCommand
                                                 ?? (filterSessionsCommand = new Command(
                                                         async () => await ExecuteFilterSessionsAsync()));

        async Task ExecuteFilterSessionsAsync()
        {
            IsBusy = true;
            NoSessionsFound = false;

            // Abort the current command if the user is typing fast
            if (!string.IsNullOrEmpty(Filter))
            {
                var query = Filter;
                await Task.Delay(250);
                if (query != Filter) return;
            }

            SessionsFiltered.ReplaceRange(Sessions.Search(Filter));
            SortSessions();

            if (SessionsGrouped.Count == 0)
            {
                NoSessionsFoundMessage = "No Sessions Found";
                NoSessionsFound = true;
            }
            else
            {
                NoSessionsFound = false;
            }

            IsBusy = false;
        }



        ICommand loadSessionsCommand;

        public ICommand LoadSessionsCommand => loadSessionsCommand
                                               ?? (loadSessionsCommand = new Command<bool>(
                                                       async (f) => await ExecuteLoadSessionsAsync()));


        async Task<bool> ExecuteLoadSessionsAsync(bool force = false)
        {
            if (IsBusy) return false;

            try
            {
                NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                IsBusy = true;
                NoSessionsFound = false;
                Filter = string.Empty;

#if DEBUG
                await Task.Delay(1000);
#endif

                Sessions.ReplaceRange(await StoreManager.SessionStore.GetItemsAsync(force));

                SessionsFiltered.ReplaceRange(Sessions);
                SortSessions();

                if (SessionsGrouped.Count == 0)
                {
                    NoSessionsFoundMessage = "No Sessions Found";
                    NoSessionsFound = true;
                }
                else
                {
                    NoSessionsFound = false;
                }

                if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows
                    && FeatureFlags.AppLinksEnabled)
                {
                    foreach (var session in Sessions)
                    {
                        try
                        {
                            // data migration: older applinks are removed so the index is rebuilt again
                            Application.Current.AppLinks.DeregisterLink(
                                new Uri(
                                    $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory}/{session.Id}"));

                            Application.Current.AppLinks.RegisterLink(session.GetAppLink());
                        }
                        catch (Exception applinkException)
                        {
                            // don't crash the app
                            Logger.Report(applinkException, "AppLinks.RegisterLink", session.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadSessionsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }
        #endregion
    }
}

