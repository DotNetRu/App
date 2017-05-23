using System;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	public static class MiniHackExtensions
	{
		public static AppLinkEntry GetAppLink(this MiniHack miniHack)
		{
			var url = $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.MiniHacksSiteSubdirectory.ToLowerInvariant()}/{miniHack.Id}";

			var entry = new AppLinkEntry
			{
				Title = miniHack.Name ?? "",
				Description = miniHack.Description ?? "",
				AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
				IsLinkActive = true
			};

			if (Device.OS == TargetPlatform.iOS)
			{
				if (!string.IsNullOrEmpty(miniHack.BadgeUrl))
				{
					entry.Thumbnail = ImageSource.FromUri(miniHack.BadgeUri);
				}
				else
				{
					entry.Thumbnail = ImageSource.FromFile("Icon.png");
				}
			}

			entry.KeyValues.Add("contentType", "Session");
			entry.KeyValues.Add("appName", AboutThisApp.AppName);
			entry.KeyValues.Add("companyName", AboutThisApp.CompanyName);

			return entry;
		}
	}
}
