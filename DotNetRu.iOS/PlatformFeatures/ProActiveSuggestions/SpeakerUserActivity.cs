using System.Linq;
using System.Threading.Tasks;
using CoreSpotlight;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions;

[assembly: Dependency(typeof(SpeakerUserActivity))]

namespace XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions
{
    using DotNetRu.DataStore.Audit.DataObjects;

    using XamarinEvolve.Utils.Helpers;

	public class SpeakerUserActivity : IPlatformSpecificExtension<Speaker>
	{
		private NSUserActivity _activity;

		public Task Execute(Speaker entity)
		{
			if (this._activity != null)
			{
			    this._activity.Invalidate();
			}

		    this._activity = new NSUserActivity($"{AboutThisApp.PackageName}.speaker")
			{
				Title = entity.FullName
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

		void RegisterHandoff(Speaker speaker)
		{
			var userInfo = new NSMutableDictionary();
			userInfo.Add(new NSString("Url"), new NSString(speaker.GetAppLink().AppLinkUri.AbsoluteUri));

			var keywords = new NSMutableSet<NSString>(new NSString(speaker.FirstName), new NSString(speaker.LastName));
			if (speaker.Sessions != null)
			{
				foreach (var session in speaker.Sessions)
				{
					keywords.Add(new NSString(session.Title));
				}
			}

		    this._activity.Keywords = new NSSet<NSString>(keywords);
		    this._activity.WebPageUrl = NSUrl.FromString(speaker.GetWebUrl());

		    this._activity.EligibleForHandoff = false;

		    this._activity.AddUserInfoEntries(userInfo);

			// Provide context
			var attributes = new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.speaker");
			attributes.Keywords = keywords.ToArray().Select(k => k.ToString()).ToArray();
			attributes.Url = NSUrl.FromString(speaker.GetAppLink().AppLinkUri.AbsoluteUri);
		    this._activity.ContentAttributeSet = attributes;
		}
	}
}
