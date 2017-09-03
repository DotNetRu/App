using System;
using XamarinEvolve.DataObjects;
using System.Threading.Tasks;
using Xamarin.Forms;
using FormsToolkit;
using System.Linq;
using XamarinEvolve.DataStore.Azure.Abstractions;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	public class FavoriteService
	{
		Session sessionQueued;
		public FavoriteService()
		{
			MessagingService.Current.Subscribe(MessageKeys.LoggedIn, async (s) =>
				{
					if (sessionQueued == null)
						return;

					await ToggleFavorite(sessionQueued);
				});
		}

		private bool _busy;

		public async Task<bool> ToggleFavorite(Session session)
		{
			if (!Settings.Current.IsLoggedIn)
			{
				sessionQueued = session;
				MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
				return false;
			}

			if (_busy)
			{
				return false;
			}

			_busy = true;
			sessionQueued = null;

			try
			{
				var actionWrapper = DependencyService.Get<IPlatformActionWrapper<Session>>();
				actionWrapper?.Before(session);

				var store = DependencyService.Get<IFavoriteStore>();
				var targetValue = !session.IsFavorite;
				session.IsFavorite = targetValue; //switch first so UI updates :)

				actionWrapper?.After(session);

				if (!targetValue)
				{
					DependencyService.Get<ILogger>().Track(EvolveLoggerKeys.FavoriteRemoved, "Title", session.Title);
				}

				// always clean up existing rows to be sure
				var items = await store.GetItemsAsync().ConfigureAwait(false);
				foreach (var item in items.Where(s => s.SessionId == session.Id))
				{
					await store.RemoveAsync(item).ConfigureAwait(false);
				}

				if (targetValue)
				{
					DependencyService.Get<ILogger>().Track(EvolveLoggerKeys.FavoriteAdded, "Title", session.Title);
					await store.InsertAsync(new Favorite { SessionId = session.Id });
				}

				Settings.Current.LastFavoriteTime = DateTime.UtcNow;

				var dataHandler = DependencyService.Get<IPlatformSpecificDataHandler<Session>>();
				if (dataHandler != null)
				{
					await dataHandler.UpdateSingleEntity(session).ConfigureAwait(false);
				}

				session.IsFavorite = targetValue;
				MessagingService.Current.SendMessage(MessageKeys.SessionFavoriteToggled, session);
				return true;
			}
			catch (Exception e)
			{
				DependencyService.Get<ILogger>().Report(e, "ToggleFavoriteSession", session.Id, Severity.Error);
				return false;
			}
			finally
			{
				_busy = false;
			}
		}
	}
}

