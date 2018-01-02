using DotNetRu.Clients.Portable.Model;
using DotNetRu.Utils.Helpers;
using MvvmHelpers;

namespace DotNetRu.Clients.Portable.ViewModel
{
    public class AboutViewModel : SettingsViewModel
    {
        public AboutViewModel()
        {
            this.AboutItems.Clear();
            this.AboutItems.Add(new LocalizableMenuItem { Name = "About this app", Icon = "icon_venue.png" });

            if (!FeatureFlags.SponsorsOnTabPage)
            {
                this.InfoItems.Add(new LocalizableMenuItem { Name = "Friends", Icon = "icon_venue.png", Parameter = "sponsors" });
            }

            // pushItem.Command = new Command(
            // () =>
            // {
            // if (push.IsRegistered)
            // {
            // UpdateItems();
            // return;
            // }

            // if (Settings.AttemptedPush)
            // {
            // MessagingService.Current.SendMessage<MessagingServiceQuestion>(
            // MessageKeys.Question,
            // new MessagingServiceQuestion
            // {
            // Title = "Push Notification",
            // Question =
            // "To enable push notifications, please go into Settings, Tap Notifications, and set Allow Notifications to on.",
            // Positive = "Settings",
            // Negative = "Maybe Later",
            // OnCompleted = (result) =>
            // {
            // if (result)
            // {
            // push.OpenSettings();
            // }
            // }
            // });
            // return;
            // }

            // push.RegisterForNotifications();
            // });
        }

        public ObservableRangeCollection<MenuItem> InfoItems { get; } = new ObservableRangeCollection<MenuItem>();
    }
}