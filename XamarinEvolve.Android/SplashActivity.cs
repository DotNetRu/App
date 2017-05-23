﻿
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
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Droid
{
    [Activity(Label = XamarinEvolve.Utils.EventInfo.EventShortName, Icon = "@drawable/newicon", Theme="@style/SplashTheme", MainLauncher=true)]            

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

