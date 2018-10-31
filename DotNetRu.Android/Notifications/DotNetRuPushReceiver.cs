namespace DotNetRu.Droid.Notifications
{
    using Android.App;
    using Android.Content;
    using Android.Runtime;

    [Preserve]
    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new[] { "${applicationId}" })]
    public class DotNetRuPushReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // TODO use once AppCenter supports silent pushes
        }
    }
}