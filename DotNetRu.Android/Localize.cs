using System.Globalization;
using System.Threading;
using Android.Content;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable.Helpers;
using XamarinEvolve.Clients.Portable.Interfaces;

[assembly: Dependency(typeof(DotNetRu.Droid.Localize))]

namespace DotNetRu.Droid
{
    public class Localize : ILocalize
    {
        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
        public CultureInfo GetCurrentCultureInfo()
        {
           
            var androidLocale = Java.Util.Locale.Default;
            var language = (androidLocale.ToString().ToLower().Contains("ru"))? "ru" : "en";
            return new CultureInfo(language);
        }
    }
}