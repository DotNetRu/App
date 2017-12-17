namespace XamarinEvolve.Clients.Portable
{
    using System.ComponentModel.DataAnnotations;
    using XamarinEvolve.Utils.Helpers;

    public enum Language
    {
        [Display(Name = "Русский")]
        [LanguageCode(LanguageCode = "ru")]
        Russian,

        [Display(Name = "English")]
        [LanguageCode(LanguageCode = "en")]
        English
    }
}