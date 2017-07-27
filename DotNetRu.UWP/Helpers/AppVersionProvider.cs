using Windows.ApplicationModel;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.UWP.Helpers;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace XamarinEvolve.UWP.Helpers
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string AppVersion
        {
            get
            {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}
