using Android.Content;
using Android.Content.PM;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Droid.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace DotNetRu.Droid.Helpers
{
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