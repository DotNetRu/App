namespace XamarinEvolve.Clients.Portable.Helpers
{
    using System.Linq;

    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    using XamarinEvolve.Utils.Helpers;

    /// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

	    private static readonly Language CurrentLanguageDefault = Language.English;

	    public static Language? CurrentLanguage
	    {
	        get
	        {
	            string languageCode = AppSettings.GetValueOrDefault(        
	                nameof(CurrentLanguage),
	                string.Empty);

	            if (languageCode == string.Empty)
	            {
	                return null;
	            }

	            return EnumExtension.GetEnumValues<Language>().Single(x => x.GetLanguageCode() == languageCode);
	        }
	        set
	        {
	            string languageCode = value.GetLanguageCode();
	            AppSettings.AddOrUpdateValue(nameof(CurrentLanguage), languageCode);
	        }
	    }
    }
}