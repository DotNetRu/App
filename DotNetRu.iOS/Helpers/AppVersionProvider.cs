using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.iOS.Helpers;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace DotNetRu.iOS.Helpers
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string AppVersion
        {
            get
            {
                var version = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleShortVersionString")].ToString();
                var buildNumber = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();

                return version + " (" + buildNumber + ")";
            }
        }
    }
}

