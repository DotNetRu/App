using System.Threading.Tasks;
using System.Linq;
using CoreSpotlight;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;
using XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions;
using System;
using XamarinEvolve.Utils;

[assembly: Dependency(typeof(SessionUserActivity))]

namespace XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions
{
	public class SessionUserActivity : IPlatformSpecificExtension<Session>
	{
		private NSUserActivity _activity;

		public Task Execute(Session entity)
		{
			if (_activity != null)
			{
				_activity.Invalidate();
			}

			_activity = new NSUserActivity($"{AboutThisApp.PackageName}.session")
			{
				Title = entity.Title,
			};

			RegisterHandoff(entity);

			_activity.BecomeCurrent();

			return Task.CompletedTask;
		}

		public Task Finish()
		{
			_activity?.ResignCurrent();
			return Task.CompletedTask;
		}

		void RegisterHandoff(Session session)
		{
			var userInfo = new NSMutableDictionary();
			var uri = new NSString(session.GetAppLink().AppLinkUri.AbsoluteUri);

			userInfo.Add(new NSString("link"), uri);
			userInfo.Add(new NSString("Url"), uri);

			var keywords = new NSMutableSet<NSString>(new NSString(session.Title));
			foreach (var speaker in session.Speakers)
			{
				keywords.Add(new NSString(speaker.FullName));
			}
			foreach (var category in session.Categories)
			{
				keywords.Add(new NSString(category.Name));
			}

			_activity.Keywords = new NSSet<NSString>(keywords);
			_activity.WebPageUrl = NSUrl.FromString(session.GetWebUrl());
			_activity.UserInfo = userInfo;

			// Provide context
			var attributes = new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.session");
			attributes.Keywords = keywords.ToArray().Select(k => k.ToString()).ToArray();
			attributes.Url = NSUrl.FromString(session.GetAppLink().AppLinkUri.AbsoluteUri);
			if (session.StartTime.HasValue && session.StartTime > DateTime.MinValue)
			{
				attributes.DueDate = session.StartTime.Value.ToNSDate();
				attributes.StartDate = session.StartTime.Value.ToNSDate();
				attributes.EndDate = session.EndTime.Value.ToNSDate();

				attributes.ImportantDates = new[] { attributes.StartDate, attributes.EndDate };
			}
			_activity.ContentAttributeSet = attributes;
			_activity.EligibleForHandoff = true;
		}
	}
}
