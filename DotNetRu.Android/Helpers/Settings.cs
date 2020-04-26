using Xamarin.Essentials;

namespace DotNetRu.Droid.Helpers
{
    internal static class Settings
    {
        private const int NotificationIdDefault = 0;

        internal static int NotificationID
        {
            get => Preferences.Get(nameof(NotificationID), NotificationIdDefault);

            set => Preferences.Set(nameof(NotificationID), value);
        }

        internal static int GetUniqueNotificationID()
        {
            return NotificationID++;
        }
    }
}
