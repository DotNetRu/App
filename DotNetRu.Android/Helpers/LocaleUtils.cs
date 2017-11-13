using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace DotNetRu.Droid.Helpers
{
    public class LocaleUtils
    {

        private static Locale currentLocale;
        public static void SetLocale(Locale locale)
        {
            currentLocale = locale;
            if (currentLocale != null)
            {
                Locale.Default = currentLocale;
            }
        }

        public static void UpdateConfig(ContextThemeWrapper wrapper)
        {
            if (currentLocale != null)
            {
                Configuration configuration = new Configuration {Locale = currentLocale};
                wrapper.ApplyOverrideConfiguration(configuration);
            }
        }

        public static void UpdateConfig(Application app, Configuration configuration)
        {
            if (currentLocale != null)
            {
                //Wrapping the configuration to avoid Activity endless loop
                Configuration config = new Configuration(configuration);
                config.Locale = currentLocale;
                Resources res = app.BaseContext.Resources;
                if (res.DisplayMetrics != null)
                {
                    res.UpdateConfiguration(config, res.DisplayMetrics);
                }
            }
        }

    }
}