namespace XamarinEvolve.Droid
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Support.V7.App;

    using DotNetRu.Droid;

    using XamarinEvolve.Utils.Helpers;

    [Activity(
        Label = AboutThisApp.AppName,
        Icon = "@drawable/ic_launcher",
        Theme = "@style/SplashTheme",
        MainLauncher = true)]

    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
            this.Finish();
        }
    }
}

