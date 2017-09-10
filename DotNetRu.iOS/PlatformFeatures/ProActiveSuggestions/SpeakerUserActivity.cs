using System.Linq;
using System.Threading.Tasks;
using CoreSpotlight;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;
using XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions;
using XamarinEvolve.Utils;

[assembly: Dependency(typeof(SpeakerUserActivity))]

namespace XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions
{
	using XamarinEvolve.Utils.Helpers;

	public class SpeakerUserActivity : IPlatformSpecificExtension<Speaker>
	{
		private NSUserActivity _activity;

		public Task Execute(Speaker entity)
		{
			if (_activity != null)
			{
				_activity.Invalidate();
			}

			_activity = new NSUserActivity($"{AboutThisApp.PackageName}.speaker")
			{
				Title = entity.FullName
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

			_activity.Keywords = new NSSet<NSString>(keywords);
			_activity.WebPageUrl = NSUrl.FromString(speaker.GetWebUrl());

			_activity.EligibleForHandoff = false;

			_activity.AddUserInfoEntries(userInfo);

			// Provide context
			var attributes = new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.speaker");
			attributes.Keywords = keywords.ToArray().Select(k => k.ToString()).ToArray();
			attributes.Url = NSUrl.FromString(speaker.GetAppLink().AppLinkUri.AbsoluteUri);
			_activity.ContentAttributeSet = attributes;
		}
	}
}
