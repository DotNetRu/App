namespace DotNetRu.iOS
{
    using System;
    using System.ComponentModel;

    using CoreSpotlight;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.UI;
    using DotNetRu.iOS.Renderers;
    using DotNetRu.Utils;
    using DotNetRu.Utils.Helpers;
    using FFImageLoading.Forms.Platform;
    using FFImageLoading.Transformations;

    using FormsToolkit;
    using FormsToolkit.iOS;

    using Foundation;

    using ImageCircle.Forms.Plugin.iOS;

    using Social;

    using UIKit;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        private static UIColor primaryColor;

        /// <inheritdoc />
        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);
            ((App)Xamarin.Forms.Application.Current).SecondOnResume();
        }

        /// <inheritdoc />
        public override bool HandleOpenURL(UIApplication application, NSUrl url)
        {
            if (!string.IsNullOrEmpty(url.AbsoluteString))
            {
                ((App)Xamarin.Forms.Application.Current).SendOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (!string.IsNullOrEmpty(url.AbsoluteString))
            {
                ((App)Xamarin.Forms.Application.Current).SendOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OpenUrl(
            UIApplication application,
            NSUrl url,
            string sourceApplication,
            NSObject annotation)
        {
            if (!string.IsNullOrEmpty(url.AbsoluteString))
            {
                ((App)Xamarin.Forms.Application.Current).SendOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();

            InitializeDependencies();

            this.LoadApplication(new App());

            InitializeThemeColors();

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, this.DidBecomeActive);

            // Get possible shortcut item
            if (launchOptions != null)
            {
                this.LaunchedShortcutItem =
                    launchOptions[UIApplication.LaunchOptionsShortcutItemKey] as UIApplicationShortcutItem;
            }

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private static void InitializeDependencies()
        {
            Toolkit.Init();

            ImageCircleRenderer.Init();
            NonScrollableListViewRenderer.Initialize();
            SelectedTabPageRenderer.Initialize();

            CachedImageRenderer.Init();

            // this is needed to tell linker to keep this type. See https://github.com/luberda-molinet/FFImageLoading/issues/462
            var ignore = new CircleTransformation();

            // this is needed to tell linker to keep this type
            var dummy = new DateTimeOffsetConverter();
        }

        private static void InitializeThemeColors()
        {
            // Set up appearance after loading theme resources in App.xaml
            primaryColor = ((Color)Xamarin.Forms.Application.Current.Resources["Primary"]).ToUIColor();
            UINavigationBar.Appearance.BarTintColor =
                ((Color)Xamarin.Forms.Application.Current.Resources["BarBackgroundColor"]).ToUIColor();
            UINavigationBar.Appearance.TintColor = primaryColor; // Tint color of button items
            UIBarButtonItem.Appearance.TintColor = primaryColor; // Tint color of button items
            UITabBar.Appearance.TintColor = primaryColor;
            UISwitch.Appearance.OnTintColor = primaryColor;
            UIAlertView.Appearance.TintColor = primaryColor;

            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = primaryColor;
            UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = primaryColor;
            UIView.AppearanceWhenContainedIn(typeof(SLComposeViewController)).TintColor = primaryColor;
        }

        private void DidBecomeActive(NSNotification notification)
        {
            ((App)Xamarin.Forms.Application.Current).SecondOnResume();
        }

        #region Quick Action

        public UIApplicationShortcutItem LaunchedShortcutItem { get; set; }

        public override void OnActivated(UIApplication uiApplication)
        {
            Console.WriteLine("OnActivated");

            // Handle any shortcut item being selected
            this.HandleShortcutItem(this.LaunchedShortcutItem);

            // Clear shortcut after it's been handled
            this.LaunchedShortcutItem = null;
        }

        void CheckForAppLink(NSUserActivity userActivity)
        {
            var link = string.Empty;

            switch (userActivity.ActivityType)
            {
                case "NSUserActivityTypeBrowsingWeb":
                    link = userActivity.WebPageUrl.AbsoluteString;
                    break;
                case "com.apple.corespotlightitem":
                    if (userActivity.UserInfo.ContainsKey(CSSearchableItem.ActivityIdentifier))
                        link = userActivity.UserInfo.ObjectForKey(CSSearchableItem.ActivityIdentifier).ToString();
                    break;
                default:
                    if (userActivity.UserInfo.ContainsKey(new NSString("link")))
                        link = userActivity.UserInfo[new NSString("link")].ToString();
                    break;
            }

            if (!string.IsNullOrEmpty(link))
                ((App)Xamarin.Forms.Application.Current).SendOnAppLinkRequestReceived(new Uri(link));
        }

        // if app is already running
        public override void PerformActionForShortcutItem(
            UIApplication application,
            UIApplicationShortcutItem shortcutItem,
            UIOperationHandler completionHandler)
        {
            Console.WriteLine("PerformActionForShortcutItem");

            // Perform action
            var handled = this.HandleShortcutItem(shortcutItem);
            completionHandler(handled);
        }

        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            Console.WriteLine("HandleShortcutItem");
            var handled = false;

            // Anything to process?
            if (shortcutItem == null)
            {
                return false;
            }

            // Take action based on the shortcut type
            switch (shortcutItem.Type)
            {
                case ShortcutIdentifier.Tweet:
                    Console.WriteLine("QUICKACTION: Tweet");
                    var slComposer = SLComposeViewController.FromService(SLServiceType.Twitter);
                    if (slComposer == null)
                    {
                        new UIAlertView(
                            title: "Unavailable",
                            message: "Twitter is not available, please sign in on your devices settings screen.",
                            del: (IUIAlertViewDelegate)null,
                            cancelButtonTitle: "OK").Show();
                    }
                    else
                    {
                        slComposer.SetInitialText(EventInfo.HashTag);
                        if (slComposer.EditButtonItem != null)
                        {
                            slComposer.EditButtonItem.TintColor = primaryColor;
                        }

                        slComposer.CompletionHandler += result =>
                            {
                                this.InvokeOnMainThread(
                                    () => UIApplication.SharedApplication.KeyWindow.RootViewController
                                        .DismissViewController(true, null));
                            };

                        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(
                            slComposer,
                            true);
                    }

                    handled = true;
                    break;
                case ShortcutIdentifier.Announcements:
                    Console.WriteLine("QUICKACTION: Accouncements");
                    this.ContinueNavigation(AppPage.Notification);
                    handled = true;
                    break;
                case ShortcutIdentifier.Events:
                    Console.WriteLine("QUICKACTION: Meetups");
                    this.ContinueNavigation(AppPage.Meetups);
                    handled = true;
                    break;
            }

            Console.Write(handled);

            // Return results
            return handled;
        }

        private void ContinueNavigation(AppPage page, string id = null)
        {
            Console.WriteLine("ContinueNavigation");

            // TODO: display UI in Forms somehow
            Console.WriteLine("Show the page for " + page);
            MessagingService.Current.SendMessage(
                "DeepLinkPage",
                new DeepLinkPage { Page = page, Id = id });
        }

        /// <inheritdoc />
        public override void UserActivityUpdated(UIApplication application, NSUserActivity userActivity)
        {
            this.CheckForAppLink(userActivity);
        }

        /// <inheritdoc />
        public override bool ContinueUserActivity(
            UIApplication application,
            NSUserActivity userActivity,
            UIApplicationRestorationHandler completionHandler)
        {
            this.CheckForAppLink(userActivity);
            return true;
        }

        #endregion
    }
}

