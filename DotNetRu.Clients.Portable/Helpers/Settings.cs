// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace XamarinEvolve.Clients.Portable.Helpers
{
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


		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;
        public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}

			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

	    private const string CurrentLanguageKey = "";
	    private static readonly string CurrentLanguageDefault = "";

	    public static string CurrentLanguage
	    {
	        get { return AppSettings.GetValueOrDefault(CurrentLanguageKey, CurrentLanguageDefault); }
	        set { AppSettings.AddOrUpdateValue(CurrentLanguageKey, value); }
	    }
    }
}