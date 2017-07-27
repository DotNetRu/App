using System;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;
using XamarinEvolve.Utils;

[assembly:Dependency(typeof(GroupSettingsProvider))]

namespace XamarinEvolve.iOS
{
	public class GroupSettingsProvider : IPlatformSpecificSettings
	{
		private NSUserDefaults _groupSettings = new NSUserDefaults($"group.{AboutThisApp.PackageName}", NSUserDefaultsType.SuiteName);

		public string UserIdentifier
		{
			get
			{
				return _groupSettings.StringForKey(nameof(UserIdentifier));
			}
			set
			{
				_groupSettings.SetString(value, nameof(UserIdentifier));
				_groupSettings.Synchronize();
			}
		}
	}
}
