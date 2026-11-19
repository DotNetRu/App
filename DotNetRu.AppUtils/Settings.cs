namespace DotNetRu.AppUtils; 

using Xamarin.Essentials;

public static class Settings
{
    public static bool IsConnected
    {
        get => Preferences.Get(nameof(IsConnected), defaultValue: true);
        set => Preferences.Set(nameof(IsConnected), value);
    }
    public static string FilteredCategories { get; set; }
}
