namespace XamarinEvolve.Clients.Portable
{
    using System.ComponentModel.DataAnnotations;
    using XamarinEvolve.Utils.Helpers;

    public enum Language
    {
        [LanguageCode(LanguageCode = "ru")]
        Russian,

        [LanguageCode(LanguageCode = "en")]
        English
    }
}