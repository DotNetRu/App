using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetRu.Clients.Portable.Interfaces;
using Foundation;
using UIKit;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable.Helpers;

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