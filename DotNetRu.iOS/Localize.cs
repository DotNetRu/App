using System.Globalization;
using System.Threading;
using DotNetRu.Clients.Portable.Interfaces;
using Foundation;

using Xamarin.Forms;

[assembly: Dependency(typeof(DotNetRu.iOS.Localize))]

namespace DotNetRu.iOS
{
    class Localize : ILocalize
    {
        public void SetLocale(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
        public CultureInfo GetCurrentCultureInfo()
        {
            var language = "en";
            if (NSLocale.PreferredLanguages.Length > 0 &&
                NSLocale.PreferredLanguages[0].ToLower().Contains("ru"))
            {
                language = "ru";
            }
            return new CultureInfo(language);
        }
    }

}