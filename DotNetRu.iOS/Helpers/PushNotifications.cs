using System.Threading.Tasks;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.iOS.Helpers;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushNotifications))]

namespace DotNetRu.iOS.Helpers
{
    public class PushNotifications : IPushNotifications
    {
        #region IPushNotifications implementation

        public Task<bool> RegisterForNotifications()
        {
            var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet ());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            
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

