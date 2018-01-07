namespace DotNetRu.Clients.UI.Pages.Obsolete
{
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Model;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public partial class AboutPage
    {
        private readonly IPushNotifications push;

        private bool isPushRegistered = false;

        public AboutPage()
        {
            this.InitializeComponent();
            this.BindingContext = null;
            this.push = DependencyService.Get<IPushNotifications>();
            this.isPushRegistered = this.push.IsRegistered;
        }

        public override AppPage PageType => AppPage.Information;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!this.isPushRegistered && Settings.Current.AttemptedPush)
            {
                this.push.RegisterForNotifications();
            }

            this.isPushRegistered = this.push.IsRegistered;
        }

        public void OnResume()
        {
            this.OnAppearing();
        }
    }
}
// ViewModel.cs
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
