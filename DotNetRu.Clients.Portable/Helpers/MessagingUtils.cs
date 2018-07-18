using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Utils.Helpers;
using FormsToolkit;
using Xamarin.Forms;

namespace DotNetRu.Clients.Portable.Helpers
{
    public static class MessagingUtils
	{
	    public static void SendToast(string message)
	    {
	        DependencyService.Get<IToast>().SendToast(message);
        }

		public static void SendAlert(string title, string message)
		{
			MessagingService.Current.SendMessage(MessageKeys.Message, new MessagingServiceAlert
			{
				Title = title,
				Message = message,
				Cancel = "OK"
			});
		}
	}
}

