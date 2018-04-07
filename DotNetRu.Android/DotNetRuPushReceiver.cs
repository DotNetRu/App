namespace DotNetRu.Droid
{
    using Android.App;
    using Android.Content;
    using Android.Runtime;
    using Android.Util;

    using DotNetRu.DataStore.Audit.Services;

    [Preserve]
    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" },
        Categories = new[] { "${applicationId}" })]
    public class DotNetRuPushReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Debug("Push", "Push Notification receviced!");

            UpdateService.UpdateAudit();
        }
    }
}