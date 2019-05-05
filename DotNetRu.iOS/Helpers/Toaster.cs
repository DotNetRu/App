using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.iOS.Helpers;
using GlobalToast;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]
namespace DotNetRu.iOS.Helpers
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {         
            Device.BeginInvokeOnMainThread(() => { Toast.ShowToast(message); });
        }
    }
}
