using Foundation;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace XamarinEvolve.iOS
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

