using System.Threading.Tasks;
using DotNetRu.Clients.Portable.Interfaces;
using Foundation;

using UIKit;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;

[assembly:Dependency(typeof(PushNotifications))]
namespace XamarinEvolve.iOS
{
    public class PushNotifications : IPushNotifications
    {
        #region IPushNotifications implementation

        public Task<bool> RegisterForNotifications()
        {
            Settings.Current.PushNotificationsEnabled = true;
            Settings.Current.AttemptedPush = true;

            var pushSettings = UIUserNotificationSettings.GetSettingsForTypes (
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet ());

            UIApplication.SharedApplication.RegisterUserNotificationSettings (pushSettings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications ();
            
            return Task.FromResult(true);
        }

        public bool IsRegistered => UIApplication.SharedApplication.IsRegisteredForRemoteNotifications &&
                                    UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None;

        public void OpenSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

        #endregion
    }
}

