namespace DotNetRu.Droid.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    internal static class Settings
    {
        private static readonly int NotificationIdDefault = 0;

        internal static ISettings AppSettings => CrossSettings.Current;

        internal static int NotificationID
        {
            get => AppSettings.GetValueOrDefault(nameof(NotificationID), NotificationIdDefault);

            set => AppSettings.AddOrUpdateValue(nameof(NotificationID), value);
        }

        internal static int GetUniqueNotificationID()
        {
            return NotificationID++;
        }
    }
}