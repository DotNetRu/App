namespace DotNetRu.Droid
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
            this.SendRegistrationToServer(refreshedToken);
        }

        private void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}