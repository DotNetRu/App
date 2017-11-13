using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Util;
using DotNetRu.Droid.Helpers;
using Naxam.Controls.Platform.Droid;

﻿using DotNetRu.Droid.Helpers;

namespace DotNetRu.Droid
{
    using System.Reflection;

    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;

    using FormsToolkit.Droid;

    using Gcm;

    using ImageCircle.Forms.Plugin.Droid;

    using Plugin.Permissions;

    using Refractored.XamForms.PullToRefresh.Droid;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.UI;
    using XamarinEvolve.Droid;
    using XamarinEvolve.Utils.Helpers;

    using Debug = System.Diagnostics.Debug;
    [Activity(
        Label = AboutThisApp.AppName,
        Icon = "@drawable/ic_launcher",
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "http",
        DataPathPrefix = "/" + AboutThisApp.SessionsSiteSubdirectory + "/",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "http",
        DataPathPrefix = "/" + AboutThisApp.SpeakersSiteSubdirectory + "/",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataPathPrefix = "/" + AboutThisApp.SpeakersSiteSubdirectory + "/",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "http",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataPathPrefix = "/" + AboutThisApp.SessionsSiteSubdirectory + "/",
        DataHost = AboutThisApp.AppLinksBaseDomain)]

    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "http",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataHost = AboutThisApp.AppLinksBaseDomain)]
    public class MainActivity : FormsAppCompatActivity
    {
        public MainActivity()
        {
            LocaleUtils.UpdateConfig(this);
        }
        public static MainActivity Current { get; }

        // GoogleApiClient client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Forms.SetFlags("FastRenderers_Experimental");

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;
            SetupBottomTabs();

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            // TODO enable AppLinks, currently there are issues with Google Play Services
            // AndroidAppLinks.Init(this);
            Toolkit.Init();

            PullToRefreshLayoutRenderer.Init();
            typeof(Color).GetProperty("Accent", BindingFlags.Public | BindingFlags.Static)
                .SetValue(null, Color.FromHex("#757575"));

            ImageCircleRenderer.Init();

#if ENABLE_TEST_CLOUD // Mapping StyleID to element content descriptions
            Xamarin.Forms.Forms.ViewInitialized += (object sender, Xamarin.Forms.ViewInitializedEventArgs e) => {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId)) {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };
#endif
            this.LoadApplication(new App());

            var gpsAvailable = this.IsPlayServicesAvailable();
            Settings.Current.PushNotificationsEnabled = gpsAvailable;

            if (gpsAvailable)
            {
                // client = new GoogleApiClient.Builder(this)
                // .AddApi(AppIndex.API)
                // .Build();
            }

            this.OnNewIntent(this.Intent);

            if (!Settings.Current.PushNotificationsEnabled) return;
#if ENABLE_TEST_CLOUD
#else
            this.RegisterWithGCM();
#endif

            DataRefreshService.ScheduleRefresh(this);
        }

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            Debug.WriteLine("MainActivity", "Registering...");
            GcmService.Initialize(this);
            GcmService.Register(this);
        }

        public bool IsPlayServicesAvailable()
        {
            // int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            // if (resultCode != ConnectionResult.Success)
            // {
            // if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
            // {
            // if (Settings.Current.GooglePlayChecked)
            // return false;

            // Settings.Current.GooglePlayChecked = true;
            // Toast.MakeText(this, "Google Play services is not installed, push notifications have been disabled.",
            // ToastLength.Long)
            // .Show();
            // }
            // else
            // {
            // Settings.Current.PushNotificationsEnabled = false;
            // }
            // return false;
            // }
            // else
            // {
            // Settings.Current.PushNotificationsEnabled = true;
            // return true;
            // }
            return true;
        }

        void SetupBottomTabs()
        {
            var stateList = new Android.Content.Res.ColorStateList(
                new int[][] {
                    new int[] { Android.Resource.Attribute.StateChecked
                    },
                    new int[] { Android.Resource.Attribute.StateEnabled
                    }
                },
                new int[] {
                    Android.Graphics.Color.Azure, //Selected
                    Android.Graphics.Color.White //Normal
                });

            BottomTabbedRenderer.BackgroundColor = new Android.Graphics.Color(ResourcesCompat.GetColor(Resources, Resource.Color.primary, null));//Resources.GetColor(Resource.Color.primaryDark);
            BottomTabbedRenderer.FontSize = 12f;
            BottomTabbedRenderer.IconSize = 16;
            BottomTabbedRenderer.ItemTextColor = stateList;
            BottomTabbedRenderer.ItemIconTintList = stateList;
            //BottomTabbedRenderer.Typeface = Typeface.CreateFromAsset(this.Assets, "architep.ttf");
            BottomTabbedRenderer.ItemBackgroundResource = Resource.Drawable.bnv_selector;
            BottomTabbedRenderer.ItemSpacing = 4;
            BottomTabbedRenderer.ItemPadding = new Xamarin.Forms.Thickness(6);
            BottomTabbedRenderer.BottomBarHeight = 56;
            BottomTabbedRenderer.ItemAlign = ItemAlignFlags.Center;
            BottomTabbedRenderer.MenuItemIconSetter = (menuItem, iconSource, selected) =>
            {
                /*TODO: Make it without switch. Following variant does not work :(
                resId = Resources.GetIdentifier(iconSource.File, "drawable", PackageName);  //returns 0 always
                */
                if (iconSource != null)
                {
                    switch (iconSource.File)
                    {
                        case "menu_feed.png":
                            menuItem.SetIcon(Resource.Drawable.menu_feed);
                            break;
                        case "menu_speakers.png":
                            menuItem.SetIcon(Resource.Drawable.menu_speakers);
                            break;
                        case "menu_events.png":
                            menuItem.SetIcon(Resource.Drawable.menu_events);
                            break;
                        case "menu_sponsors.png":
                            menuItem.SetIcon(Resource.Drawable.menu_sponsors);
                            break;
                        case "menu_info.png":
                            menuItem.SetIcon(Resource.Drawable.menu_info);
                            break;
                    }
                }
            };
        }

        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}

