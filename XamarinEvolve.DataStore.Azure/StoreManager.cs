using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Abstractions;
using System.Collections.Generic;
using XamarinEvolve.Clients.Portable;
using System.Diagnostics;
using XamarinEvolve.Utils;

namespace XamarinEvolve.DataStore.Azure
{
    public class StoreManager : IStoreManager
    {
        
        public static MobileServiceClient MobileService { get; set; }

        /// <summary>
        /// Syncs all tables.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="syncUserSpecific">If set to <c>true</c> sync user specific.</param>
        public async Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            if(!IsInitialized)
                await InitializeAsync();
            
            var taskList = new List<Task<bool>>();
            taskList.Add(CategoryStore.SyncAsync());
            taskList.Add(NotificationStore.SyncAsync());
            taskList.Add(SpeakerStore.SyncAsync());
            taskList.Add(SessionStore.SyncAsync());
            taskList.Add(SponsorStore.SyncAsync());
            taskList.Add(EventStore.SyncAsync());

            if (syncUserSpecific)
            {
                taskList.Add(FeedbackStore.SyncAsync());
                taskList.Add(FavoriteStore.SyncAsync());
				taskList.Add(ConferenceFeedbackStore.SyncAsync());
            }

            var successes = await Task.WhenAll(taskList).ConfigureAwait(false);
            return successes.Any(x => !x);//if any were a failure.
        }

        public bool IsInitialized { get; private set; }
        #region IStoreManager implementation
        object _locker = new object ();
        public async Task InitializeAsync()
        {
            MobileServiceSQLiteStore store;
            lock(_locker) 
            {

                if (IsInitialized)
                    return;
                
                IsInitialized = true;
                var dbId = Settings.DatabaseId;
                var path = $"syncstore{dbId}.db";
                MobileService = new MobileServiceClient(ApiKeys.ApiUrl);
                store = new MobileServiceSQLiteStore (path);
                store.DefineTable<Category> ();
                store.DefineTable<Favorite> ();
                store.DefineTable<Notification> ();
                store.DefineTable<FeaturedEvent> ();
                store.DefineTable<Feedback> ();
                store.DefineTable<Room> ();
                store.DefineTable<Session> ();
                store.DefineTable<Speaker> ();
                store.DefineTable<Sponsor> ();
                store.DefineTable<SponsorLevel> ();
                store.DefineTable<StoreSettings> ();
				store.DefineTable<ConferenceFeedback>();
            }
            try
            {
                await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler()).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                Debugger.Break();
            }
        }
       
        INotificationStore _notificationStore;
        public INotificationStore NotificationStore => _notificationStore ?? (_notificationStore  = DependencyService.Get<INotificationStore>());


        ICategoryStore _categoryStore;
        public ICategoryStore CategoryStore => _categoryStore ?? (_categoryStore  = DependencyService.Get<ICategoryStore>());

        IFavoriteStore _favoriteStore;
        public IFavoriteStore FavoriteStore => _favoriteStore ?? (_favoriteStore  = DependencyService.Get<IFavoriteStore>());

        IFeedbackStore _feedbackStore;
        public IFeedbackStore FeedbackStore => _feedbackStore ?? (_feedbackStore  = DependencyService.Get<IFeedbackStore>());

		IConferenceFeedbackStore _conferenceFeedbackStore;
		public IConferenceFeedbackStore ConferenceFeedbackStore => _conferenceFeedbackStore ?? (_conferenceFeedbackStore = DependencyService.Get<IConferenceFeedbackStore>());

        ISessionStore _sessionStore;
        public ISessionStore SessionStore => _sessionStore ?? (_sessionStore  = DependencyService.Get<ISessionStore>());
     

        ISpeakerStore _speakerStore;
        public ISpeakerStore SpeakerStore => _speakerStore ?? (_speakerStore  = DependencyService.Get<ISpeakerStore>());

        IEventStore _eventStore;
        public IEventStore EventStore => _eventStore ?? (_eventStore = DependencyService.Get<IEventStore>());

        ISponsorStore _sponsorStore;
        public ISponsorStore SponsorStore => _sponsorStore ?? (_sponsorStore  = DependencyService.Get<ISponsorStore>());


		#endregion

		public async Task<MobileServiceUser> LoginAnonymouslyAsync(string impersonateUserId = null)
		{
			if (!IsInitialized)
			{
				await InitializeAsync();
			}

			if (string.IsNullOrEmpty(impersonateUserId))
			{
				var settings = await ReadSettingsAsync();
				impersonateUserId = settings?.UserId; // see if we have a saved user id from a previous token
			}

			var credentials = new JObject();
			if (!string.IsNullOrEmpty(impersonateUserId))
			{
				credentials["anonymousUserId"] = impersonateUserId;
			}
			MobileServiceUser user = await MobileService.LoginAsync("AnonymousUser", credentials);

			await CacheToken(user);

			return user;
		}

        public async Task<MobileServiceUser> LoginAsync(string username, string password)
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }

            var credentials = new JObject();
            credentials["email"] = username;
            credentials["password"] = password;

            MobileServiceUser user = await MobileService.LoginAsync("Xamarin", credentials);

            await CacheToken(user);

            return user;
        }

        public async Task LogoutAsync()
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }

            await MobileService.LogoutAsync();

            var settings = await ReadSettingsAsync();

            if (settings != null)
            {
                settings.AuthToken = string.Empty;
                settings.UserId = string.Empty;

                await SaveSettingsAsync(settings);
            }
        }

        async Task SaveSettingsAsync(StoreSettings settings) =>
            await MobileService.SyncContext.Store.UpsertAsync(nameof(StoreSettings), new[] { JObject.FromObject(settings) }, true);

        async Task<StoreSettings> ReadSettingsAsync() =>
            (await MobileService.SyncContext.Store.LookupAsync(nameof(StoreSettings), StoreSettings.StoreSettingsId))?.ToObject<StoreSettings>();
        

        async Task CacheToken(MobileServiceUser user)
        {
            var settings = new StoreSettings
            {
                UserId = user.UserId,
                AuthToken = user.MobileServiceAuthenticationToken
            };

            await SaveSettingsAsync(settings);
            
        }

		public class StoreSettings
        {
            public const string StoreSettingsId = "store_settings";

            public StoreSettings()
            {
                Id = StoreSettingsId;
            }

            public string Id { get; set; }

            public string UserId { get; set; }

            public string AuthToken { get; set; }
        }
    }
}

