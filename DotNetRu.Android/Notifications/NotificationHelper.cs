namespace DotNetRu.Droid.Notifications
{
    using Android.App;
    using Android.Content;
    using Android.Util;

    using DotNetRu.Droid.Helpers;

    using Firebase;
    using Firebase.Iid;
    using Firebase.Messaging;

    internal sealed class NotificationHelper
    {
        internal const string NewMeetupChannelID = "NewMeetup";
        internal const string NewMeetupChannelName = "New meetup information";

        internal static void CreateNotificationChannel()
        {
            NotificationChannel notificationChannel = new NotificationChannel(
                NewMeetupChannelID,
                NewMeetupChannelName,
                NotificationImportance.High);

            notificationChannel.EnableVibration(vibration: true);
            notificationChannel.LockscreenVisibility = NotificationVisibility.Public;

            NotificationManager notificationManager =
                (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

            notificationManager.CreateNotificationChannel(notificationChannel);
        }

        internal static void InitializePushNotifications()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                CreateNotificationChannel();
            }

            FirebaseApp.InitializeApp(Application.Context);
            Log.Debug("AuditUpdate", "Firebase token: " + FirebaseInstanceId.Instance.Token);

            FirebaseMessaging.Instance.SubscribeToTopic(NewMeetupChannelID);
        }

        internal static void SendNotification(Context context, string title, string body)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(context, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int PendingIntentID = 0;
            PendingIntent pendingIntent = PendingIntent.GetActivity(
                context,
                PendingIntentID,
                intent,
                PendingIntentFlags.OneShot);

            Notification.Builder notificationBuilder = new Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.ic_launcher).SetContentTitle(title).SetContentText(body)
                .SetContentIntent(pendingIntent);

            NotificationManager notificationManager =
                (NotificationManager)context.GetSystemService(Context.NotificationService);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(NewMeetupChannelID);
            }

            notificationManager.Notify(Settings.GetUniqueNotificationID(), notificationBuilder.Build());
        }
    }
}