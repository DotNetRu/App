namespace XamarinEvolve.Clients.Portable
{
    using System;

    using DotNetRu.DataStore.Audit.DataObjects;

    using Xamarin.Forms;

    using XamarinEvolve.Utils.Helpers;

    public static class SpeakerExtensions
    {
        public static AppLinkEntry GetAppLink(this Speaker speaker)
        {
            var url =
                $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory.ToLowerInvariant()}/{speaker.Id}";

            var entry = new AppLinkEntry
                            {
                                Title = speaker.FullName ?? "",
                                Description = speaker.Biography ?? "",
                                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                                IsLinkActive = true,
                            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (!string.IsNullOrEmpty(speaker.AvatarUrl))
                {
                    entry.Thumbnail = ImageSource.FromUri(speaker.AvatarUri);
                }
                else
                {
                    entry.Thumbnail = ImageSource.FromFile("Icon.png");
                }
            }

            entry.KeyValues.Add("contentType", "Speaker");
            entry.KeyValues.Add("appName", AboutThisApp.AppName);
            entry.KeyValues.Add("companyName", AboutThisApp.CompanyName);

            return entry;
        }

        public static string GetWebUrl(this Speaker speaker)
        {
            return $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory}/#{speaker.Id}";
        }
    }
}

