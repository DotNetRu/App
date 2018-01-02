using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.iOS.Helpers;
using ToastIOS;
using Xamarin.Forms;

[assembly:Dependency(typeof(Toaster))]
namespace DotNetRu.iOS.Helpers
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
