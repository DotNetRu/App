namespace DotNetRu.Clients.Portable.Helpers
{
    using System;

    using DotNetRu.Clients.Portable.ViewModel;

    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        public static Language? CurrentLanguage
        {
            get
            {
                string languageCode = AppSettings.GetValueOrDefault(nameof(CurrentLanguage), null);
                return languageCode == null ? (Language?)null : GetLanguage(languageCode);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                string languageCode = value.Value.GetLanguageCode();
                AppSettings.AddOrUpdateValue(nameof(CurrentLanguage), languageCode);
            }
        }

        private static ISettings AppSettings => CrossSettings.Current;

        private static Language GetLanguage(string languageCode)
        {
            if (languageCode == "ru")
            {
                return Language.Russian;
            }

            return Language.English;
        }
    }
}