namespace DotNetRu.Clients.Portable.Services
{
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.ViewModel;

    using Microsoft.Maui;

    public static class LanguageService
    {
        public static Language GetCurrentLanguage()
        {
            var savedLanguage = Helpers.Settings.CurrentLanguage;
            var uiLanguage = DependencyService.Get<ILocalize>().GetCurrentCultureInfo().TwoLetterISOLanguageName == "ru"
                                 ? Language.Russian
                                 : Language.English;

            return savedLanguage ?? uiLanguage;
        }
    }
}
