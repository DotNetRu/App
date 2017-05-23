﻿using System;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using XamarinEvolve.Utils;

namespace XamarinEvolve.DataStore.Mock
{
	public class NotificationStore : BaseStore<Notification>, INotificationStore
	{
		public NotificationStore()
		{
		}

		public async Task<Notification> GetLatestNotification()
		{
			var items = await GetItemsAsync();
			return items.ElementAt(0);
		}

		public override Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
		{
			var items = new[]
			{
				new Notification
				{
					Date = DateTime.UtcNow,
					Text = $"Welcome to {EventInfo.EventName}!"
				},
				new Notification
				{
					Date = DateTime.UtcNow,
					Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas fringilla felis a diam auctor tempus. Pellentesque pharetra nibh nisi. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi luctus metus felis, at mattis lorem varius in. Curabitur ut nisi id sem rutrum semper. Suspendisse potenti. Nullam malesuada a justo sed pulvinar. Nulla bibendum eros nunc, sed consectetur massa laoreet sit amet."
				},
				new Notification
				{
					Date = DateTime.UtcNow,
					Text = "Nulla tincidunt urna quis odio luctus, in imperdiet elit dapibus. Maecenas imperdiet tortor purus, id rhoncus erat tincidunt in. Phasellus tristique maximus mi, eget euismod purus pharetra eu. Mauris at consectetur eros. Nunc at sapien sit amet justo rhoncus imperdiet. Cras vel est ac nunc dapibus convallis sit amet non leo. Cras sed justo risus. Phasellus non eros consequat, imperdiet elit non, ullamcorper arcu. Quisque condimentum sem vitae elit molestie consequat.\n\nFusce tortor urna, iaculis eu ullamcorper non, vulputate non magna. Praesent venenatis, libero id rhoncus vestibulum, justo ante laoreet lectus, sed iaculis ipsum massa non ipsum."
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-4),
					Text = $"Welcome to {EventInfo.EventName}!"
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-5),
					Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas fringilla felis a diam auctor tempus. Pellentesque pharetra nibh nisi. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi luctus metus felis, at mattis lorem varius in. Curabitur ut nisi id sem rutrum semper. Suspendisse potenti. Nullam malesuada a justo sed pulvinar. Nulla bibendum eros nunc, sed consectetur massa laoreet sit amet."
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-4),
					Text = "Nulla tincidunt urna quis odio luctus, in imperdiet elit dapibus. Maecenas imperdiet tortor purus, id rhoncus erat tincidunt in. Phasellus tristique maximus mi, eget euismod purus pharetra eu. Mauris at consectetur eros. Nunc at sapien sit amet justo rhoncus imperdiet. Cras vel est ac nunc dapibus convallis sit amet non leo. Cras sed justo risus. Phasellus non eros consequat, imperdiet elit non, ullamcorper arcu. Quisque condimentum sem vitae elit molestie consequat.\n\nFusce tortor urna, iaculis eu ullamcorper non, vulputate non magna. Praesent venenatis, libero id rhoncus vestibulum, justo ante laoreet lectus, sed iaculis ipsum massa non ipsum."
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-6),
					Text = $"Welcome to {EventInfo.EventName}!"
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-7),
					Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas fringilla felis a diam auctor tempus. Pellentesque pharetra nibh nisi. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi luctus metus felis, at mattis lorem varius in. Curabitur ut nisi id sem rutrum semper. Suspendisse potenti. Nullam malesuada a justo sed pulvinar. Nulla bibendum eros nunc, sed consectetur massa laoreet sit amet."
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-7),
					Text = $"Welcome to {EventInfo.EventName}!"
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-40),
					Text = $"Welcome to {EventInfo.EventName}!"
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-40),
					Text = $"Welcome to {EventInfo.EventName}!"
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-40),
					Text = "Nulla tincidunt urna quisodio luctus, in imperdiet elit dapibus. Maecenas imperdiet tortor purus, id rhoncus erat tincidunt in. Phasellus tristique maximus mi, eget euismod purus pharetra eu. Mauris at consectetur eros. Nunc at sapien sit amet justo rhoncus imperdiet. Cras vel est ac nunc dapibus convallis sit amet non leo. Cras sed justo risus. Phasellus non eros consequat, imperdiet elit non, ullamcorper arcu. Quisque condimentum sem vitae elit molestie consequat.\n\nFusce tortor urna, iaculis eu ullamcorper non, vulputate non magna. Praesent venenatis, libero id rhoncus vestibulum, justo ante laoreet lectus, sed iaculis ipsum massa non ipsum."
				},
				new Notification
				{
					Date = DateTime.UtcNow.AddDays(-41),
					Text = "Nulla tincidunt urna quis odio luctus, in imperdiet elit dapibus. Maecenas imperdiet tortor purus, id rhoncus erat tincidunt in. Phasellus tristique maximus mi, eget euismod purus pharetra eu. Mauris at consectetur eros. Nunc at sapien sit amet justo rhoncus imperdiet. Cras vel est ac nunc dapibus convallis sit amet non leo. Cras sed justo risus. Phasellus non eros consequat, imperdiet elit non, ullamcorper arcu. Quisque condimentum sem vitae elit molestie consequat.\n\nFusce tortor urna, iaculis eu ullamcorper non, vulputate non magna. Praesent venenatis, libero id rhoncus vestibulum, justo ante laoreet lectus, sed iaculis ipsum massa non ipsum."
				},
			};
			return Task.FromResult(items as IEnumerable<Notification>);
		}
	}
}

