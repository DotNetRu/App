//using Android.App;
//using Android.Content;
//using Gcm;

//namespace XamarinEvolve.Droid
//{
//    using XamarinEvolve.Utils.Helpers;

//    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS,
//        Name = AboutThisApp.PackageName + ".GcmBroadcastReceiver")]
//    [IntentFilter(new[] {Intent.ActionBootCompleted})] // Allow GCM on boot and when app is closed   
//    [IntentFilter(new[] {Constants.INTENT_FROM_GCM_MESSAGE}, Categories = new[] {"@PACKAGE_NAME@"})]
//    [IntentFilter(new[] {Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK}, Categories = new[] {"@PACKAGE_NAME@"})]
//    [IntentFilter(new[] {Constants.INTENT_FROM_GCM_LIBRARY_RETRY}, Categories = new[] {"@PACKAGE_NAME@"})]
//    public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
//    {
//        public static string[] SENDERIDS = {ApiKeys.GoogleSenderId};
//    }
//}