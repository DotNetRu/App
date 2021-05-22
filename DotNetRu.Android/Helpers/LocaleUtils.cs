using Android.App;
using Android.Content.Res;
using Android.Views;

using Java.Util;

namespace DotNetRu.Droid.Helpers
{
    public static class LocaleUtils
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
                using Configuration configuration = new Configuration {Locale = currentLocale};
                wrapper?.ApplyOverrideConfiguration(configuration);
            }
        }

        public static void UpdateConfig(Application app, Configuration configuration)
        {
            if (currentLocale != null)
            {
                //Wrapping the configuration to avoid Activity endless loop
                using Configuration config = new Configuration(configuration) {Locale = currentLocale};
                Resources res = app?.BaseContext.Resources;
                if (res?.DisplayMetrics != null)
                {
                    res.UpdateConfiguration(config, res.DisplayMetrics);
                }
            }
        }
    }
}
