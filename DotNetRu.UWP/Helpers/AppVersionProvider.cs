using DotNetRu.UWP.Helpers;

using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace DotNetRu.UWP.Helpers
{
    using Windows.ApplicationModel;

    using DotNetRu.Clients.Portable.Interfaces;

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