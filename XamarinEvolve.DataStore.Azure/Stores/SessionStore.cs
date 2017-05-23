﻿using System;
using XamarinEvolve.DataStore.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using XamarinEvolve.DataObjects;
using System.Linq;

using XamarinEvolve.DataStore.Azure;
using Xamarin.Forms;
using XamarinEvolve.Utils;

namespace XamarinEvolve.DataStore.Azure
{
    public class SessionStore : BaseStore<Session>, ISessionStore
    {

        public override async Task<IEnumerable<Session>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore().ConfigureAwait (false);
            if (forceRefresh)
                await PullLatestAsync().ConfigureAwait (false);

            var sessions = await Table.OrderBy(s => s.StartTime).ToListAsync().ConfigureAwait(false);
            var favStore = DependencyService.Get<IFavoriteStore>();
            await favStore.GetItemsAsync(true).ConfigureAwait(false);//pull latest

            foreach (var session in sessions)
            {
                var isFav = await favStore.IsFavorite(session.Id).ConfigureAwait(false);
                session.IsFavorite = isFav;
            }

			var dataShare = DependencyService.Get<IPlatformSpecificDataHandler<Session>>();
			if (dataShare != null)
			{
				await dataShare.UpdateMultipleEntities(sessions).ConfigureAwait(false);
			}

            return sessions;
        }

        public async Task<IEnumerable<Session>> GetSpeakerSessionsAsync(string speakerId)
        {
            
            await InitializeStore().ConfigureAwait (false);

            var speakers = await GetItemsAsync().ConfigureAwait(false);

            return speakers.Where(s => s.Speakers != null && s.Speakers.Any(speak => speak.Id == speakerId))
                .OrderBy(s => s.StartTimeOrderBy);
        }

        public async Task<IEnumerable<Session>> GetNextSessions(int maxNumber = 2)
        {
            var date = DateTime.UtcNow.AddMinutes(-30);//about to start in next 30

            var sessions = await GetItemsAsync().ConfigureAwait(false);

			var result = sessions.Where(s => s.StartTimeOrderBy.ToUniversalTime() > date && s.IsFavorite).Take(maxNumber);
           
            var enumerable = result as Session[] ?? result.ToArray();
            return enumerable.Any() ? enumerable : null;
        }

        public async Task<Session> GetAppIndexSession (string id)
        {
            await InitializeStore ().ConfigureAwait (false);
            var sessions = await Table.Where (s => s.Id == id || s.RemoteId == id).ToListAsync();

            if (sessions == null || sessions.Count == 0)
                return null;
            
            return sessions [0];
        }

        public override string Identifier => "Session";
    }
}

