using DotNetRu.Clients.Portable.Interfaces;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Droid;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace XamarinEvolve.Droid
{
    using Android.Content;
    using Android.Content.PM;

    public class AppVersionProvider : IAppVersionProvider
    {
        public string AppVersion
        {
            get
            {
                Context context = Android.App.Application.Context;
                PackageInfo packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                return packageInfo.VersionName + " (" + packageInfo.VersionCode + ")";
            }
        }
    }
}