using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using UIKit;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable.Helpers;
using XamarinEvolve.Clients.Portable.Interfaces;

[assembly: Dependency(typeof(DotNetRu.iOS.Localize))]

namespace DotNetRu.iOS
{
    class Localize : ILocalize
    {
        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
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