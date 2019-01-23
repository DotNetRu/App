namespace DotNetRu.Droid.Notifications
{
    using Android.App;
    using Android.Content;
    using Android.Util;
    using DotNetRu.Clients.UI;
    using DotNetRu.Droid.Helpers;

    using Firebase;
    using Firebase.Iid;
    using Firebase.Messaging;

    internal sealed class NotificationHelper
    {
        internal static void CreateNotificationChannel(string channelID)
        {           
            var notificationChannel = new NotificationChannel(
                channelID,
                "New meetup information",
                NotificationImportance.High);

            notificationChannel.EnableVibration(vibration: true);
            notificationChannel.LockscreenVisibility = NotificationVisibility.Public;

            var notificationManager =
                (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

            notificationManager.CreateNotificationChannel(notificationChannel);
        }

        internal static void InitializePushNotifications()
        {
            var config = AppConfig.GetConfig();

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                CreateNotificationChannel(config.PushNotificationsChannel);
            }

            FirebaseApp.InitializeApp(Application.Context);
            Log.Debug("AuditUpdate", "Firebase token: " + FirebaseInstanceId.Instance.Token);

            FirebaseMessaging.Instance.SubscribeToTopic(config.PushNotificationsChannel);
        }

        internal static void SendNotification(Context context, string title, string body)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            var intent = new Intent(context, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int PendingIntentID = 0;
            var pendingIntent = PendingIntent.GetActivity(
                context,
                PendingIntentID,
                intent,
                PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.menu_events)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(autoCancel: true);

            var notificationManager =
                (NotificationManager)context.GetSystemService(Context.NotificationService);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                var config = AppConfig.GetConfig();
                notificationBuilder.SetChannelId(config.PushNotificationsChannel);
            }

            notificationManager.Notify(Settings.GetUniqueNotificationID(), notificationBuilder.Build());
        }
    }
}
