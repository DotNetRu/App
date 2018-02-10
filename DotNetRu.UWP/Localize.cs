using Xamarin.Forms;

[assembly: Dependency(typeof(DotNetRu.UWP.Localize))]

namespace DotNetRu.UWP
{
    using System.Globalization;
    using System.Threading;

    using DotNetRu.Clients.Portable.Interfaces;

    public class Localize : ILocalize
    {
        public void SetLocale(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var language = "en";
            return new CultureInfo(language);
        }
    }
}