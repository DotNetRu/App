using FormsToolkit;
using Xamarin.Forms;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	using XamarinEvolve.Utils.Helpers;

	public static class MessagingUtils
	{
		public static void SendOfflineMessage()
		{
			var toaster = DependencyService.Get<IToast>();
			toaster.SendToast("You are currently offline, please connect to the internet and try again.");
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

