
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using DotNetRu.Droid;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Droid
{
	using XamarinEvolve.Utils.Helpers;

	[Activity(Label = EventInfo.EventShortName, Icon = "@drawable/ic_launcher", Theme="@style/SplashTheme", MainLauncher=true)]            

    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}

