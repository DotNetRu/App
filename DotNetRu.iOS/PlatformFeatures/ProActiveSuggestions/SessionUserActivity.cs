using System;
using System.Linq;
using System.Threading.Tasks;
using CoreSpotlight;

using DotNetRu.Clients.Portable.Model.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.iOS.PlatformFeatures.ProActiveSuggestions;
using DotNetRu.Utils.Helpers;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(SessionUserActivity))]

namespace DotNetRu.iOS.PlatformFeatures.ProActiveSuggestions
{
    using Xamarin.Forms.Platform.iOS;

    public class SessionUserActivity
    {
        private NSUserActivity activity;

        public Task Execute(TalkModel entity)
        {
            this.activity?.Invalidate();

            this.activity = new NSUserActivity($"{AboutThisApp.PackageName}.session") { Title = entity.Title, };

            this.RegisterHandoff(entity);

            this.activity.BecomeCurrent();

            return Task.CompletedTask;
        }

        public Task Finish()
        {
            this.activity?.ResignCurrent();
            return Task.CompletedTask;
        }

        private void RegisterHandoff(TalkModel talkModel)
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

            this.activity.Keywords = new NSSet<NSString>(keywords);
            this.activity.WebPageUrl = NSUrl.FromString(talkModel.GetWebUrl());
            this.activity.UserInfo = userInfo;

            // Provide context
            var attributes =
                new CSSearchableItemAttributeSet($"{AboutThisApp.PackageName}.session")
                    {
                        Keywords =
                            keywords.ToArray()
                                .Select(
                                    k =>
                                        k.ToString())
                                .ToArray(),
                        Url = NSUrl.FromString(
                            talkModel.GetAppLink()
                                .AppLinkUri
                                .AbsoluteUri)
                    };
            if (talkModel.StartTime.HasValue && talkModel.StartTime > DateTime.MinValue)
            {
                attributes.DueDate = talkModel.StartTime.Value.ToNSDate();
                attributes.StartDate = talkModel.StartTime.Value.ToNSDate();
                attributes.EndDate = talkModel.EndTime?.ToNSDate();

                attributes.ImportantDates = new[] { attributes.StartDate, attributes.EndDate };
            }

            this.activity.ContentAttributeSet = attributes;
            this.activity.EligibleForHandoff = true;
        }
    }
}
