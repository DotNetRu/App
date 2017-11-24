using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Droid;

[assembly: Dependency(typeof(Toaster))]

namespace XamarinEvolve.Droid
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