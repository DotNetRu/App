namespace DotNetRu.Droid
{
    using System.Threading.Tasks;

    using Android.App;
    using Android.Content;
    using Android.Util;

    using DotNetRu.DataStore.Audit.Services;
    using DotNetRu.Droid.Helpers;

    using Firebase.Messaging;

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class DotNetRuFirebaseMessagingService : FirebaseMessagingService
    {
        public const string AuditUpdateChannel = "com.dotnetru.app";

        public override async void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug("Push", "AuditUpdate. Push Notification receviced!");

            await UpdateService.UpdateAudit().ContinueWith(
                t => this.SendNotification(
                    Application.Context,
                    "New information",
                    "New DotNetRu information is available"),
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void SendNotification(Context context, string title, string body)
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
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetContentIntent(pendingIntent);

            NotificationManager notificationManager =
                (NotificationManager)context.GetSystemService(NotificationService);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                string channelName = "New data";
                var importance = NotificationImportance.High;
                NotificationChannel notificationChannel =
                    new NotificationChannel(AuditUpdateChannel, channelName, importance);

                notificationChannel.EnableVibration(vibration: true);
                notificationChannel.LockscreenVisibility = NotificationVisibility.Public;

                notificationManager.CreateNotificationChannel(notificationChannel);

                notificationBuilder.SetChannelId(AuditUpdateChannel);
            }

            notificationManager.Notify(Settings.GetUniqueNotificationID(), notificationBuilder.Build());
        }
    }
}