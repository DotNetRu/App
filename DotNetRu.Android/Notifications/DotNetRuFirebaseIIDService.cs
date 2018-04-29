namespace DotNetRu.Droid.Notifications
{
    using Android.App;
    using Android.Util;

    using Firebase.Iid;

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class DotNetRuFirebaseIIDService : FirebaseInstanceIdService
    {
        private const string Tag = "AuditUpdate";

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(Tag, "Refreshed token: " + refreshedToken);
        }
    }
}