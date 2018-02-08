using DotNetRu.UWP.Helpers;

using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]

namespace DotNetRu.UWP.Helpers
{
    using DotNetRu.Clients.Portable.Interfaces;

    using Windows.UI.Popups;

    using Xamarin.Forms;

    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var dialog = new MessageDialog(message);
                dialog.ShowAsync();
            });
        }
    }
}
