using System;
using System.Linq;
using System.Threading.Tasks;

using CoreSpotlight;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model.Extensions;
using Foundation;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions;

[assembly: Dependency(typeof(SessionUserActivity))]

namespace XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions
{
    using DotNetRu.DataStore.Audit.Models;

    using XamarinEvolve.Utils.Helpers;

	public class SessionUserActivity : IPlatformSpecificExtension<TalkModel>
	{
		private NSUserActivity _activity;

		public Task Execute(TalkModel entity)
		{
			if (this._activity != null)
			{
			    this._activity.Invalidate();
			}

		    this._activity = new NSUserActivity($"{AboutThisApp.PackageName}.session")
			{
				Title = entity.Title,
			};

		    this.RegisterHandoff(entity);

		    this._activity.BecomeCurrent();

			return Task.CompletedTask;
		}

		public Task Finish()
		{
		    this._activity?.ResignCurrent();
			return Task.CompletedTask;
		}

		void RegisterHandoff(TalkModel talkModel)
		{
			var userInfo = new NSMutableDictionary();
			var uri = new NSString(talkModel.GetAppLink().AppLinkUri.AbsoluteUri);

			userInfo.Add(new NSString("link"), uri);
			userInfo.Add(new NSString("Url"), uri);

			var keywords = new NSMutableSet<NSString>(new NSString(talkModel.Title));
			foreach (var speaker in talkModel.Speakers)
			{
				keywords.Add(new NSString(speaker.FullName));
			}

			foreach (var category in talkModel.Categories)
			{
				keywords.Add(new NSString(category.Name));
			}

		    this._activity.Keywords = new NSSet<NSString>(keywords);
		    this._activity.WebPageUrl = NSUrl.FromString(talkModel.GetWebUrl());
		    this._activity.UserInfo = userInfo;

			// Provide context
			var attributes = new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.session");
			attributes.Keywords = keywords.ToArray().Select(k => k.ToString()).ToArray();
			attributes.Url = NSUrl.FromString(talkModel.GetAppLink().AppLinkUri.AbsoluteUri);
			if (talkModel.StartTime.HasValue && talkModel.StartTime > DateTime.MinValue)
			{
				attributes.DueDate = talkModel.StartTime.Value.ToNSDate();
				attributes.StartDate = talkModel.StartTime.Value.ToNSDate();
				attributes.EndDate = talkModel.EndTime.Value.ToNSDate();

				attributes.ImportantDates = new[] { attributes.StartDate, attributes.EndDate };
			}

		    this._activity.ContentAttributeSet = attributes;
		    this._activity.EligibleForHandoff = true;
		}
	}
}
