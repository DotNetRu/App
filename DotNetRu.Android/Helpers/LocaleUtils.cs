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

        private static Locale sLocale;
        public static void setLocale(Locale locale)
        {
            sLocale = locale;
            if (sLocale != null)
            {
                Locale.Default = sLocale;
            }
        }

        public static void updateConfig(ContextThemeWrapper wrapper)
        {
            if (sLocale != null)
            {
                Configuration configuration = new Configuration();
                configuration.Locale = sLocale;
                wrapper.ApplyOverrideConfiguration(configuration);
            }
        }

        public static void updateConfig(Application app, Configuration configuration)
        {
            if (sLocale != null)
            {
                //Wrapping the configuration to avoid Activity endless loop
                Configuration config = new Configuration(configuration);
                // We must use the now-deprecated config.locale and res.updateConfiguration here,
                // because the replacements aren't available till API level 24 and 17 respectively.
                config.Locale = sLocale;
                Resources res = app.BaseContext.Resources;
                if (res.DisplayMetrics != null)
                {
                    res.UpdateConfiguration(config, res.DisplayMetrics);
                }
            }
        }

    }
}