using DotNetRu.iOS.Helpers;
using DotNetRu.Utils.Helpers;
using DotNetRu.Utils.Interfaces;
using Foundation;
using Xamarin.Forms;

[assembly:Dependency(typeof(GroupSettingsProvider))]

namespace DotNetRu.iOS.Helpers
{
    public class GroupSettingsProvider : IPlatformSpecificSettings
	{
		private readonly NSUserDefaults _groupSettings = new NSUserDefaults($"group.{AboutThisApp.PackageName}", NSUserDefaultsType.SuiteName);

		public string UserIdentifier
		{
			get => this._groupSettings.StringForKey(nameof(this.UserIdentifier));

		    set
			{
			    this._groupSettings.SetString(value, nameof(this.UserIdentifier));
			    this._groupSettings.Synchronize();
			}
		}
	}
}
