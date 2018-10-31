namespace DotNetRu.Clients.Portable.Model.Extensions
{
    using System;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;

    using Xamarin.Forms;

    public static class SpeakerExtensions
    {
        public static AppLinkEntry GetAppLink(this SpeakerModel speakerModel)
        {
            var url =
                $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/{speakerModel.Id}";

            var entry = new AppLinkEntry
                            {
                                Title = speakerModel.FullName ?? string.Empty,
                                Description = speakerModel.Biography ?? string.Empty,
                                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                                IsLinkActive = true,
                            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                entry.Thumbnail = ImageSource.FromFile("Icon.png");
            }

            entry.KeyValues.Add("contentType", "Speaker");
            entry.KeyValues.Add("appName", AboutThisApp.AppName);
            entry.KeyValues.Add("companyName", AboutThisApp.CompanyName);

            return entry;
        }

        public static string GetWebUrl(this SpeakerModel speakerModel)
        {
            return $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory}/#{speakerModel.Id}";
        }
    }
}
