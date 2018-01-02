using DotNetRu.Utils.Helpers;
using DotNetRu.Utils.Interfaces;
using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;

[assembly:Dependency(typeof(GroupSettingsProvider))]

namespace XamarinEvolve.iOS
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
