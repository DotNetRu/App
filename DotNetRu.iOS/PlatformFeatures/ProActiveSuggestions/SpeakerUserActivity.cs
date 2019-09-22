using System.Linq;
using System.Threading.Tasks;
using CoreSpotlight;

using DotNetRu.Clients.Portable.Model.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.iOS.PlatformFeatures.ProActiveSuggestions;
using DotNetRu.Utils.Helpers;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpeakerUserActivity))]

namespace DotNetRu.iOS.PlatformFeatures.ProActiveSuggestions
{
    public class SpeakerUserActivity
    {
        private NSUserActivity activity;

        public Task Execute(SpeakerModel entity)
        {
            this.activity?.Invalidate();

            this.activity = new NSUserActivity($"{AboutThisApp.PackageName}.speaker") { Title = entity.FullName };

            this.RegisterHandoff(entity);

            this.activity.BecomeCurrent();

            return Task.CompletedTask;
        }

        public Task Finish()
        {
            this.activity?.ResignCurrent();
            return Task.CompletedTask;
        }

        private void RegisterHandoff(SpeakerModel speakerModel)
        {
            var userInfo = new NSMutableDictionary
                               {
                                   {
                                       new NSString("Url"),
                                       new NSString(
                                           speakerModel.GetAppLink().AppLinkUri.AbsoluteUri)
                                   }
                               };

            var keywords = new NSMutableSet<NSString>(
                new NSString(speakerModel.FullName));
            if (speakerModel.Talks != null)
            {
                foreach (var session in speakerModel.Talks)
                {
                    keywords.Add(new NSString(session.Title));
                }
            }

            this.activity.Keywords = new NSSet<NSString>(keywords);
            this.activity.WebPageUrl = NSUrl.FromString(speakerModel.GetWebUrl());

            this.activity.EligibleForHandoff = false;

            this.activity.AddUserInfoEntries(userInfo);

            // Provide context
            var attributes =
                new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.speaker")
                    {
                        Keywords =
                            keywords.ToArray()
                                .Select(
                                    k =>
                                        k.ToString())
                                .ToArray(),
                        Url = NSUrl.FromString(
                            speakerModel
                                .GetAppLink()
                                .AppLinkUri
                                .AbsoluteUri)
                    };
            this.activity.ContentAttributeSet = attributes;
        }
    }
}
