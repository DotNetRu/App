﻿using System;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using XamarinEvolve.Utils;

namespace XamarinEvolve.DataStore.Azure
{
	public class NotificationStore : BaseStore<Notification>, INotificationStore
	{
		public NotificationStore()
		{
		}

		public async Task<Notification> GetLatestNotification()
		{
			var items = await GetItemsAsync(true);
			return items.OrderByDescending(s => s.Date).ElementAt(0);
		}

		public override async Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
		{
			var server = await base.GetItemsAsync(forceRefresh).ConfigureAwait(false);
			if (server.Count() == 0)
			{
				var items = new[]
					{
					new Notification
					{
						Date = DateTime.UtcNow.AddDays(-2),
						Text = $"Don't forget to favorite your sessions so you are ready for {EventInfo.EventName}!"
					}
				};
				return items;
			}
			return server.OrderByDescending(s => s.Date);
		}

		public override string Identifier => "Notification";
	}
}

