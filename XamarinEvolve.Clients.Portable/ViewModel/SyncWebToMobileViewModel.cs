using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Connectivity;
using XamarinEvolve.DataStore.Abstractions;
using FormsToolkit;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	public class SyncWebToMobileViewModel : ViewModelBase
	{
		public SyncWebToMobileViewModel(INavigation navigation) : base(navigation)
		{
		}

		public string Explanation => $"The {EventInfo.EventName} app and website store your favorites and feedback under an anonymous account. " +
		$"You can link the {EventInfo.EventName} app to the same account used on the website with this manual step.";

		public string Explanation2 => $"By clicking 'Link website data' below, a QR-code scanner will appear that allows you to scan the QR-code on the {EventInfo.EventName} website.\n\n" +
		"WARNING: The app will REMOVE your currently stored data and sync the favorites and feedback you have stored on the website.";

		public async Task SyncWebToMobileAsync(string userId)
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				MessagingUtils.SendOfflineMessage();
				return;
			}

			if (IsBusy)
				return;

			IsBusy = true;
			try
			{
				Logger.Track(EvolveLoggerKeys.SyncWebToMobile);

				var ssoClient = DependencyService.Get<ISSOClient>();
				await ssoClient.LogoutAsync();

				// login with the new user Id obtained via the QR code scan
				var account = await ssoClient.LoginAnonymouslyAsync(userId);
				if (account != null)
				{
					Settings.Current.UserIdentifier = account.User.Email;

					MessagingService.Current.SendMessage(MessageKeys.LoggedIn);
					Logger.Track(EvolveLoggerKeys.LoginSuccess);

					Settings.Current.FirstRun = false;
				}

				try
				{
					var storeManager = DependencyService.Get<IStoreManager>();
					await storeManager.SyncAllAsync(Settings.Current.IsLoggedIn);
					Settings.Current.LastSync = DateTime.UtcNow;
					Settings.Current.HasSyncedData = true;
				}
				catch (Exception ex)
				{
					//if sync doesn't work don't worry it is alright we can recover later
					Logger.Report(ex);
				}
			}
			catch (Exception ex)
			{
				Logger.Report(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}

