﻿using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.iOS;
using ToastIOS;
using UIKit;
using CoreGraphics;

[assembly:Dependency(typeof(Toaster))]
namespace XamarinEvolve.iOS
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
					Toast.MakeText(message, Toast.LENGTH_LONG)
				         .SetCornerRadius(5)
				         .SetGravity(ToastGravity.Top)
				         .Show(ToastType.Warning);
                });
        }
    }
}
