namespace DotNetRu.Clients.Portable.ViewModel
{
    public enum Language
    {
        Russian,

        English
    }

    public static class LanguageExtensions
    {
        public static string GetLanguageCode(this Language language)
        {
            switch (language)
            {
                case Language.Russian:
                    return "ru";
                case Language.English:
                    return "en";
                default:
                    return "en";
            }
        }
    }
}