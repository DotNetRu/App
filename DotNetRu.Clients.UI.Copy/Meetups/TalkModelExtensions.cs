namespace DotNetRu.Clients.Portable.Model.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;

    using Microsoft.Maui;

    public static class TalkModelExtensions
    {
        public static AppLinkEntry GetAppLink(this TalkModel talkModel)
        {
            var url =
                $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory.ToLowerInvariant()}/{talkModel.Id}";

            var entry = new AppLinkEntry
                            {
                                Title = talkModel.Title ?? string.Empty,
                                Description = talkModel.Abstract ?? string.Empty,
                                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                                IsLinkActive = true
                            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                entry.Thumbnail = ImageSource.FromFile("Icon.png");
            }

            entry.KeyValues.Add("contentType", "Session");
            entry.KeyValues.Add("appName", AboutThisApp.AppName);
            entry.KeyValues.Add("companyName", AboutThisApp.CompanyName);

            return entry;
        }

        public static string GetWebUrl(this TalkModel talkModel)
        {
            return
                $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory}/#{talkModel.Id}";
        }

        public static IEnumerable<TalkModel> Search(this IEnumerable<TalkModel> sessions, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return sessions;
            }

            var searchSplit = searchText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // search title, then category, then speaker name
            return sessions.Where(
                session => searchSplit.Any(
                    search => session.Haystack.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}
