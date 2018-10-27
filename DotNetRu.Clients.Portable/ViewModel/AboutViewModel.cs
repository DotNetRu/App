namespace XamarinEvolve.Clients.Portable
{
    using MvvmHelpers;

    using XamarinEvolve.Utils;

    public class AboutViewModel : SettingsViewModel
    {
        public ObservableRangeCollection<Grouping<string, MenuItem>> MenuItems { get; }

        public ObservableRangeCollection<MenuItem> InfoItems { get; } = new ObservableRangeCollection<MenuItem>();

        public ObservableRangeCollection<MenuItem> AccountItems { get; } = new ObservableRangeCollection<MenuItem>();

        public AboutViewModel()
        {
            AboutItems.Clear();
            AboutItems.Add(new MenuItem { Name = "About this app", Icon = "icon_venue.png" });

            if (!FeatureFlags.SponsorsOnTabPage)
            {
                InfoItems.Add(new MenuItem { Name = "Friends", Icon = "icon_venue.png", Parameter = "sponsors" });
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
    }
}

