using System;
using System.Linq;
using System.Threading.Tasks;
using CoreSpotlight;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;
using XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions;
using XamarinEvolve.Utils;

[assembly: Dependency(typeof(MiniHackUserActivity))]

namespace XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions
{
	public class MiniHackUserActivity : IPlatformSpecificExtension<MiniHack>
	{
		private NSUserActivity _activity;

		public Task Execute(MiniHack entity)
		{
			if (_activity != null)
			{
				_activity.Invalidate();
			}

			_activity = new NSUserActivity($"{AboutThisApp.PackageName}.minihack")
			{
				Title = entity.Name,
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

		void RegisterHandoff(MiniHack entity)
		{
			var userInfo = new NSMutableDictionary();
			var uri = new NSString(entity.GetAppLink().AppLinkUri.AbsoluteUri);

			userInfo.Add(new NSString("link"), uri);
			userInfo.Add(new NSString("Url"), uri);

			var keywords = new NSMutableSet<NSString>(new NSString(entity.Name));
			_activity.Keywords = new NSSet<NSString>(keywords);
			_activity.UserInfo = userInfo;

			// Provide context
			var attributes = new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.minihack");
			attributes.Keywords = keywords.ToArray().Select(k => k.ToString()).ToArray();
			attributes.Url = NSUrl.FromString(entity.GetAppLink().AppLinkUri.AbsoluteUri);

			_activity.ContentAttributeSet = attributes;
			_activity.EligibleForHandoff = false;
		}
	}
}
