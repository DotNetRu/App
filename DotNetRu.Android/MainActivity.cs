using System.Reflection;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.AppIndexing;
//using Android.Gms.Common;
//using Android.Gms.Common.Apis;
using Android.OS;
using Android.Widget;
using FormsToolkit.Droid;
using Gcm;
using Plugin.Permissions;
using Refractored.XamForms.PullToRefresh.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Clients.UI;
using Xamarin;
using XamarinEvolve.Droid;

namespace DotNetRu.Droid
{
	using XamarinEvolve.Utils.Helpers;

	[Activity(Label = EventInfo.EventShortName, Icon = "@drawable/ic_launcher",
    LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "http",
    DataPathPrefix = "/" + AboutThisApp.SessionsSiteSubdirectory + "/",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "http",
    DataPathPrefix = "/" + AboutThisApp.SpeakersSiteSubdirectory + "/",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "https",
    DataPathPrefix = "/" + AboutThisApp.SpeakersSiteSubdirectory + "/",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "http",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "https",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "https",
    DataPathPrefix = "/" + AboutThisApp.SessionsSiteSubdirectory + "/",
    DataHost = AboutThisApp.AppLinksBaseDomain)]

  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "http",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  [IntentFilter(new[] {Intent.ActionView},
    Categories = new[]
    {
      Android.Content.Intent.CategoryDefault,
      Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = "https",
    DataHost = AboutThisApp.AppLinksBaseDomain)]
  public class MainActivity : FormsAppCompatActivity
  {
    private static MainActivity current;

    public static MainActivity Current
    {
      get { return current; }
    }

    // GoogleApiClient client;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
      FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

      base.OnCreate(savedInstanceState);

      Forms.Init(this, savedInstanceState);
      FormsMaps.Init(this, savedInstanceState);
           // AndroidAppLinks.Init(this);
      Toolkit.Init();

      PullToRefreshLayoutRenderer.Init();
      typeof(Color).GetProperty("Accent", BindingFlags.Public | BindingFlags.Static)
        .SetValue(null, Color.FromHex("#757575"));

      ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();

#if ENABLE_TEST_CLOUD
//Mapping StyleID to element content descriptions
            Xamarin.Forms.Forms.ViewInitialized += (object sender, Xamarin.Forms.ViewInitializedEventArgs e) => {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId)) {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };
#endif

#if !ENABLE_TEST_CLOUD
      InitializeHockeyApp();
#endif

      LoadApplication(new App());

      var gpsAvailable = IsPlayServicesAvailable();
      Settings.Current.PushNotificationsEnabled = gpsAvailable;

      if (gpsAvailable)
      {
        //client = new GoogleApiClient.Builder(this)
        //  .AddApi(AppIndex.API)
        //  .Build();
      }

      OnNewIntent(Intent);

      if (!Settings.Current.PushNotificationsEnabled)
        return;
#if ENABLE_TEST_CLOUD
#else
      RegisterWithGCM();
#endif

      DataRefreshService.ScheduleRefresh(this);
    }



    void InitializeHockeyApp()
    {
      if (string.IsNullOrWhiteSpace(ApiKeys.HockeyAppAndroid) ||
          ApiKeys.HockeyAppAndroid == nameof(ApiKeys.HockeyAppAndroid))
        return;

      HockeyApp.Android.CrashManager.Register(this, ApiKeys.HockeyAppAndroid);

      HockeyApp.Android.Metrics.MetricsManager.Register(this, Application,
        ApiKeys.HockeyAppAndroid);

    }

    private void RegisterWithGCM()
    {
      // Check to ensure everything's set up right
      GcmClient.CheckDevice(this);
      GcmClient.CheckManifest(this);

      // Register for push notifications
      System.Diagnostics.Debug.WriteLine("MainActivity", "Registering...");
      GcmService.Initialize(this);
      GcmService.Register(this);
    }

    public bool IsPlayServicesAvailable()
    {
      //int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
      //if (resultCode != ConnectionResult.Success)
      //{
      //  if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
      //  {
      //    if (Settings.Current.GooglePlayChecked)
      //      return false;

      //    Settings.Current.GooglePlayChecked = true;
      //    Toast.MakeText(this, "Google Play services is not installed, push notifications have been disabled.",
      //        ToastLength.Long)
      //      .Show();
      //  }
      //  else
      //  {
      //    Settings.Current.PushNotificationsEnabled = false;
      //  }
      //  return false;
      //}
      //else
      //{
      //  Settings.Current.PushNotificationsEnabled = true;
      //  return true;
      //}
        return true;
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
      PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

  }
}

