using System.Linq;
using System.Threading.Tasks;
using CoreSpotlight;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model.Extensions;
using DotNetRu.Utils.Helpers;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions;

[assembly: Dependency(typeof(SpeakerUserActivity))]

namespace XamarinEvolve.iOS.PlatformFeatures.ProActiveSuggestions
{
    using DotNetRu.DataStore.Audit.Models;

    public class SpeakerUserActivity : IPlatformSpecificExtension<SpeakerModel>
	{
		private NSUserActivity _activity;

		public Task Execute(SpeakerModel entity)
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

		void RegisterHandoff(SpeakerModel speakerModel)
		{
			var userInfo = new NSMutableDictionary();
			userInfo.Add(new NSString("Url"), new NSString(speakerModel.GetAppLink().AppLinkUri.AbsoluteUri));

			var keywords = new NSMutableSet<NSString>(new NSString(speakerModel.FirstName), new NSString(speakerModel.LastName));
			if (speakerModel.Talks != null)
			{
				foreach (var session in speakerModel.Talks)
				{
					keywords.Add(new NSString(session.Title));
				}
			}

		    this._activity.Keywords = new NSSet<NSString>(keywords);
		    this._activity.WebPageUrl = NSUrl.FromString(speakerModel.GetWebUrl());

		    this._activity.EligibleForHandoff = false;

		    this._activity.AddUserInfoEntries(userInfo);

			// Provide context
			var attributes = new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.speaker");
			attributes.Keywords = keywords.ToArray().Select(k => k.ToString()).ToArray();
			attributes.Url = NSUrl.FromString(speakerModel.GetAppLink().AppLinkUri.AbsoluteUri);
		    this._activity.ContentAttributeSet = attributes;
		}
	}
}
