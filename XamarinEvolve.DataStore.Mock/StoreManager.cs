﻿using System;
using XamarinEvolve.DataStore.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinEvolve.DataStore.Mock
{
    public class StoreManager : IStoreManager
    {
        #region IStoreManager implementation

        public Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            return Task.FromResult(true);
        }

        public bool IsInitialized { get { return true; }  }
        public Task InitializeAsync()
        {
            return Task.FromResult(true);
        }

        #endregion

        public Task DropEverythingAsync()
        {
            return Task.FromResult(true);
        }

        INotificationStore notificationStore;
        public INotificationStore NotificationStore => notificationStore ?? (notificationStore  = DependencyService.Get<INotificationStore>());

        IMiniHacksStore miniHacksStore;
        public IMiniHacksStore MiniHacksStore => miniHacksStore ?? (miniHacksStore  = DependencyService.Get<IMiniHacksStore>());


        ICategoryStore categoryStore;
        public ICategoryStore CategoryStore => categoryStore ?? (categoryStore  = DependencyService.Get<ICategoryStore>());

        IFavoriteStore favoriteStore;
        public IFavoriteStore FavoriteStore => favoriteStore ?? (favoriteStore  = DependencyService.Get<IFavoriteStore>());

        IFeedbackStore feedbackStore;
        public IFeedbackStore FeedbackStore => feedbackStore ?? (feedbackStore  = DependencyService.Get<IFeedbackStore>());

		IConferenceFeedbackStore conferenceFeedbackStore;
		public IConferenceFeedbackStore ConferenceFeedbackStore => conferenceFeedbackStore ?? (conferenceFeedbackStore = DependencyService.Get<IConferenceFeedbackStore>());

        ISessionStore sessionStore;
        public ISessionStore SessionStore => sessionStore ?? (sessionStore  = DependencyService.Get<ISessionStore>());

        ISpeakerStore speakerStore;
        public ISpeakerStore SpeakerStore => speakerStore ?? (speakerStore  = DependencyService.Get<ISpeakerStore>());

        IEventStore eventStore;
        public IEventStore EventStore => eventStore ?? (eventStore = DependencyService.Get<IEventStore>());

        ISponsorStore sponsorStore;
        public ISponsorStore SponsorStore => sponsorStore ?? (sponsorStore  = DependencyService.Get<ISponsorStore>());

    }
}

