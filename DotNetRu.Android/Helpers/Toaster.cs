using Android.Widget;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Droid.Helpers;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]

namespace DotNetRu.Droid.Helpers
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
            Device.BeginInvokeOnMainThread(() => { Toast.MakeText(context, message, ToastLength.Long).Show(); });
        }
    }
}