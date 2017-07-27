using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Droid;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace XamarinEvolve.Droid
{
	public class AppVersionProvider : IAppVersionProvider
	{
		public string AppVersion
		{
			get
			{
				var context = Android.App.Application.Context;
				return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
			}
		}
	}
}

