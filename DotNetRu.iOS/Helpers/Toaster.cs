using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.iOS.Helpers;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]

namespace DotNetRu.iOS.Helpers
{
    public class Toaster : IToast
    {
        private const double LongDelay = 3.5;

        public void SendToast(string message)
        {
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            var dummy = NSTimer.CreateScheduledTimer(LongDelay, (obj) =>
            {
                DismissMessage(alert, obj);
            });
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        private static void DismissMessage(UIAlertController alert, NSTimer alertDelay)
        {
            alert?.DismissViewController(true, null);
            alertDelay?.Dispose();
        }
    }
}