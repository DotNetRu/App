namespace DotNetRu.Droid
{
    using System.Reflection;

    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;
    using Android.Util;

    using DotNetRu.Droid.Helpers;
    using DotNetRu.Utils.Helpers;

    using FFImageLoading.Forms.Droid;

    using Firebase;
    using Firebase.Iid;
    using Firebase.Messaging;

    using FormsToolkit.Droid;

    using ImageCircle.Forms.Plugin.Droid;

    using Naxam.Controls.Platform.Droid;

    using Plugin.Permissions;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    [Activity(
        Label = AboutThisApp.AppName,
        Icon = "@drawable/ic_launcher",
        LaunchMode = LaunchMode.SingleTask,
        Theme = "@style/SplashTheme",
        MainLauncher = true,
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
#if ENABLE_LIVE_PLAYER
#else
            this.SetTheme(Resource.Style.MyTheme);
            Forms.SetFlags("FastRenderers_Experimental");

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;
            this.SetupBottomTabs();
#endif

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            // TODO enable AppLinks, currently there are issues with Google Play Services
            // AndroidAppLinks.Init(this);
            Toolkit.Init();

            typeof(Color).GetProperty("Accent", BindingFlags.Public | BindingFlags.Static)
                ?.SetValue(null, Color.FromHex("#757575"));

            ImageCircleRenderer.Init();

            CachedImageRenderer.Init(enableFastRenderer: true);

            FirebaseApp.InitializeApp(Android.App.Application.Context);
            Log.Debug("AuditUpdate", "Firebase token: " + FirebaseInstanceId.Instance.Token);

            FirebaseMessaging.Instance.SubscribeToTopic("AuditUpdate");

            this.LoadApplication(new Clients.UI.App());
            this.OnNewIntent(this.Intent);
        }

        private void SetupBottomTabs()
        {
            BottomTabbedRenderer.ItemPadding = new Thickness(6, 6, 6, 1);
#if ENABLE_LIVE_PLAYER
#else
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
                            case "menu_info.png":
                                menuItem.SetIcon(Resource.Drawable.menu_info);
                                break;
                        }
                    }
                };
#endif
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