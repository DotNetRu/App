namespace DotNetRu.Droid.Notifications
{
    using System.Threading.Tasks;

    using Android.App;
    using Android.Util;

    using DotNetRu.DataStore.Audit.Services;

    using Firebase.Messaging;

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class DotNetRuFirebaseMessagingService : FirebaseMessagingService
    {
        public override async void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug("Push", "AuditUpdate. Push Notification receviced!");

            await UpdateService.UpdateAudit().ContinueWith(
                t => NotificationHelper.SendNotification(
                    Application.Context,
                    "New information",
                    "New DotNetRu information is available"),
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}