namespace XamarinEvolve.iOS
{
    using System;
    using System.Collections.Generic;

    using CoreSpotlight;

    using FormsToolkit;
    using FormsToolkit.iOS;

    using Foundation;

    using Google.AppIndexing;

    using Refractored.XamForms.PullToRefresh.iOS;

    using Social;

    using UIKit;

    using Xamarin;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.UI;
    using XamarinEvolve.DataStore.Mock.Abstractions;
    using XamarinEvolve.Utils.Helpers;

    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public static class ShortcutIdentifier
        {
            public const string Tweet = AboutThisApp.PackageName + ".tweet";

            public const string Announcements = AboutThisApp.PackageName + ".announcements";

            public const string Events = AboutThisApp.PackageName + ".events";
        }

        internal static UIColor PrimaryColor = null;

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
#if !ENABLE_TEST_CLOUD
            if (!string.IsNullOrWhiteSpace(ApiKeys.HockeyAppiOS) && ApiKeys.HockeyAppiOS != nameof(ApiKeys.HockeyAppiOS))
            {
               
                var manager = BITHockeyManager.SharedHockeyManager;
                manager.Configure(ApiKeys.HockeyAppiOS);

                //Disable update manager
                manager.DisableUpdateManager = true;

                manager.StartManager();
                //manager.Authenticator.AuthenticateInstallation();
                   
            }
#endif
            // Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            //Mapping StyleId to iOS Labels
            Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) =>
                {
                    if (null != e.View.StyleId)
                    {
                        e.NativeView.AccessibilityIdentifier = e.View.StyleId;
                    }
                };
#endif

            Forms.Init();

            SetMinimumBackgroundFetchInterval();

            InitializeDependencies();

            LoadApplication(new App());

            InitializeThemeColors();

            // Process any potential notification data from launch
            ProcessNotification(launchOptions);

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, DidBecomeActive);

            var shouldPerformAdditionalDelegateHandling = true;

            // Get possible shortcut item
            if (launchOptions != null)
            {
                LaunchedShortcutItem =
                    launchOptions[UIApplication.LaunchOptionsShortcutItemKey] as UIApplicationShortcutItem;
                shouldPerformAdditionalDelegateHandling = (LaunchedShortcutItem == null);
            }

            return base.FinishedLaunching(uiApplication, launchOptions); // && shouldPerformAdditionalDelegateHandling;
        }

        void DidBecomeActive(NSNotification notification)
        {
            ((XamarinEvolve.Clients.UI.App)Xamarin.Forms.Application.Current).SecondOnResume();
        }

        static void InitializeDependencies()
        {
            FormsMaps.Init();
            Toolkit.Init();

            AppIndexing.SharedInstance.RegisterApp(PublicationSettings.iTunesAppId);

            SQLitePCL.CurrentPlatform.Init();
            Plugin.Share.ShareImplementation.ExcludedUIActivityTypes = new List<NSString>
                                                                           {
                                                                               UIActivityType
                                                                                   .PostToFacebook,
                                                                               UIActivityType
                                                                                   .AssignToContact,
                                                                               UIActivityType
                                                                                   .OpenInIBooks,
                                                                               UIActivityType
                                                                                   .PostToVimeo,
                                                                               UIActivityType
                                                                                   .PostToFlickr,
                                                                               UIActivityType
                                                                                   .SaveToCameraRoll
                                                                           };
            ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init();
            NonScrollableListViewRenderer.Initialize();
            SelectedTabPageRenderer.Initialize();
            TextViewValue1Renderer.Init();
            PullToRefreshLayoutRenderer.Init();
        }

        static void InitializeThemeColors()
        {
            // Set up appearance after loading theme resources in App.xaml
            PrimaryColor = ((Color)Xamarin.Forms.Application.Current.Resources["Primary"]).ToUIColor();
            UINavigationBar.Appearance.BarTintColor =
                ((Color)Xamarin.Forms.Application.Current.Resources["BarBackgroundColor"]).ToUIColor();
            UINavigationBar.Appearance.TintColor = PrimaryColor; //Tint color of button items
            UIBarButtonItem.Appearance.TintColor = PrimaryColor; //Tint color of button items
            UITabBar.Appearance.TintColor = PrimaryColor;
            UISwitch.Appearance.OnTintColor = PrimaryColor;
            UIAlertView.Appearance.TintColor = PrimaryColor;

            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = PrimaryColor;
            UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = PrimaryColor;
            UIView.AppearanceWhenContainedIn(typeof(SLComposeViewController)).TintColor = PrimaryColor;
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);
            ((XamarinEvolve.Clients.UI.App)Xamarin.Forms.Application.Current).SecondOnResume();
        }

        public override void RegisteredForRemoteNotifications(UIApplication app, NSData deviceToken)
        {

#if ENABLE_TEST_CLOUD
#else

            if (ApiKeys.AzureServiceBusUrl == nameof(ApiKeys.AzureServiceBusUrl))
                return;

            // Connection string from your azure dashboard
            var cs = SBConnectionString.CreateListenAccess(
                new NSUrl(ApiKeys.AzureServiceBusUrl),
                ApiKeys.AzureKey);

            // Register our info with Azure
            var hub = new SBNotificationHub (cs, ApiKeys.AzureHubName);
            hub.RegisterNativeAsync (deviceToken, null, err => {
                if (err != null)
                    Console.WriteLine("Error: " + err.Description);
                else
                    Console.WriteLine("Success");
            });
#endif
        }

        public override void ReceivedRemoteNotification(UIApplication app, NSDictionary userInfo)
        {
            // Process a notification received while the app was already open
            ProcessNotification(userInfo);
        }

        public override bool HandleOpenURL(UIApplication application, NSUrl url)
        {
            if (!string.IsNullOrEmpty(url.AbsoluteString))
            {
                ((XamarinEvolve.Clients.UI.App)App.Current).SendOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
                return true;
            }
            return false;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (!string.IsNullOrEmpty(url.AbsoluteString))
            {
                ((XamarinEvolve.Clients.UI.App)App.Current).SendOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
                return true;
            }
            return false;
        }

        public override bool OpenUrl(
            UIApplication application,
            NSUrl url,
            string sourceApplication,
            NSObject annotation)
        {
            if (!string.IsNullOrEmpty(url.AbsoluteString))
            {
                ((XamarinEvolve.Clients.UI.App)App.Current).SendOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
                return true;
            }
            return false;
        }

        void ProcessNotification(NSDictionary userInfo)
        {
            if (userInfo == null) return;

            Console.WriteLine("Received Notification");

            var apsKey = new NSString("aps");

            if (userInfo.ContainsKey(apsKey))
            {
                var alertKey = new NSString("alert");

                var aps = (NSDictionary)userInfo.ObjectForKey(apsKey);

                if (aps.ContainsKey(alertKey))
                {
                    var alert = (NSString)aps.ObjectForKey(alertKey);

                    try
                    {

                        var avAlert = new UIAlertView($"{EventInfo.EventName} Update", alert, null, "OK", null);
                        avAlert.Show();

                    }
                    catch (Exception ex)
                    {

                    }

                    Console.WriteLine("Notification: " + alert);
                }
            }
        }

        #region Quick Action

        public UIApplicationShortcutItem LaunchedShortcutItem { get; set; }

        public override void OnActivated(UIApplication uiApplication)
        {
            Console.WriteLine("OnActivated");

            // Handle any shortcut item being selected
            HandleShortcutItem(LaunchedShortcutItem);



            // Clear shortcut after it's been handled
            LaunchedShortcutItem = null;
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
                ((XamarinEvolve.Clients.UI.App)App.Current).SendOnAppLinkRequestReceived(new Uri(link));
        }

        // if app is already running
        public override void PerformActionForShortcutItem(
            UIApplication application,
            UIApplicationShortcutItem shortcutItem,
            UIOperationHandler completionHandler)
        {
            Console.WriteLine("PerformActionForShortcutItem");
            // Perform action
            var handled = HandleShortcutItem(shortcutItem);
            completionHandler(handled);
        }

        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            Console.WriteLine("HandleShortcutItem ");
            var handled = false;

            // Anything to process?
            if (shortcutItem == null) return false;

            // Take action based on the shortcut type
            switch (shortcutItem.Type)
            {
                case ShortcutIdentifier.Tweet:
                    Console.WriteLine("QUICKACTION: Tweet");
                    var slComposer = SLComposeViewController.FromService(SLServiceType.Twitter);
                    if (slComposer == null)
                    {
                        new UIAlertView(
                            "Unavailable",
                            "Twitter is not available, please sign in on your devices settings screen.",
                            null,
                            "OK").Show();
                    }
                    else
                    {
                        slComposer.SetInitialText(EventInfo.HashTag);
                        if (slComposer.EditButtonItem != null)
                        {
                            slComposer.EditButtonItem.TintColor = PrimaryColor;
                        }
                        slComposer.CompletionHandler += (result) =>
                            {
                                InvokeOnMainThread(
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
                    ContinueNavigation(AppPage.Notification);
                    handled = true;
                    break;
                case ShortcutIdentifier.Events:
                    Console.WriteLine("QUICKACTION: Events");
                    ContinueNavigation(AppPage.Events);
                    handled = true;
                    break;
            }

            Console.Write(handled);
            // Return results
            return handled;
        }

        void ContinueNavigation(AppPage page, string id = null)
        {
            Console.WriteLine("ContinueNavigation");

            // TODO: display UI in Forms somehow
            System.Console.WriteLine("Show the page for " + page);
            MessagingService.Current.SendMessage<DeepLinkPage>(
                "DeepLinkPage",
                new DeepLinkPage { Page = page, Id = id });
        }

        public override void UserActivityUpdated(UIApplication application, NSUserActivity userActivity)
        {
            CheckForAppLink(userActivity);
        }

        public override bool ContinueUserActivity(
            UIApplication application,
            NSUserActivity userActivity,
            UIApplicationRestorationHandler completionHandler)
        {
            CheckForAppLink(userActivity);
            return true;
        }

        #endregion

        #region Background Refresh

        private void SetMinimumBackgroundFetchInterval()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
        }

        // Minimum number of seconds between a background refresh this is shorter than Android because it is easily killed off.
        // 20 minutes = 20 * 60 = 1200 seconds
        private const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 1200;

        // Called whenever your app performs a background fetch
        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Do Background Fetch
            var downloadSuccessful = false;
            var logger = DependencyService.Get<ILogger>();

            try
            {
                Xamarin.Forms.Forms.Init(); //need for dependency services
                // Download data
                var manager = DependencyService.Get<IStoreManager>();
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "PerformFetch";
                logger.Report(ex);
            }

            // If you don't call this, your application will be terminated by the OS.
            // Allows OS to collect stats like data cost and power consumption
            var resultFlag = downloadSuccessful ? UIBackgroundFetchResult.NewData : UIBackgroundFetchResult.Failed;
            completionHandler(resultFlag);
        }

        #endregion
    }
}

