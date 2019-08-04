namespace DotNetRu.Droid
{

    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;

    using DotNetRu.Droid.Helpers;
    using DotNetRu.Utils.Helpers;
    using FFImageLoading.Forms.Platform;
    using FormsToolkit.Droid;

    using ImageCircle.Forms.Plugin.Droid;

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
            this.SetTheme(Resource.Style.MyTheme);

            Forms.SetFlags("FastRenderers_Experimental");

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            // TODO enable AppLinks, currently there are issues with Google Play Services
            // AndroidAppLinks.Init(this);
            Toolkit.Init();

            ImageCircleRenderer.Init();

            CachedImageRenderer.Init(enableFastRenderer: true);

            this.LoadApplication(new Clients.UI.App());
            this.OnNewIntent(this.Intent);
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
