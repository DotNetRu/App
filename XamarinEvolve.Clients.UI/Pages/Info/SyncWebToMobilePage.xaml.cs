using System;
using FormsToolkit;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace XamarinEvolve.Clients.UI
{
	public partial class SyncWebToMobilePage : BasePage
	{
		public override AppPage PageType => AppPage.SyncWebToMobile;

		static readonly string PermissionMessage = $"To link the {EventInfo.EventName} website to this app you will need to scan a code and the camera permission is required.";
		ZXingScannerPage scanPage;
		SyncWebToMobileViewModel vm;

		public SyncWebToMobilePage()
		{
			InitializeComponent();

			BindingContext = vm = new SyncWebToMobileViewModel(Navigation);

			scanPage = new ZXingScannerPage(new MobileBarcodeScanningOptions { AutoRotate = false, })
			{
				DefaultOverlayTopText = "Align the QR-code within the frame",
				DefaultOverlayBottomText = string.Empty
			};

			scanPage.OnScanResult += ScanPage_OnScanResult;

			scanPage.Title = "Scan Code";

			var item = new ToolbarItem
			{
				Text = "Cancel",
				Command = new Command(async () =>
					{
						scanPage.IsScanning = false;
						await Navigation.PopAsync();
					})
			};

			if (Device.OS != TargetPlatform.iOS)
				item.Icon = "toolbar_close.png";

			scanPage.ToolbarItems.Add(item);

		}

		async void SyncWebToMobileButton_Clicked(object sender, EventArgs e)
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
			bool request = false;
			if (status == PermissionStatus.Denied)
			{
				if (Device.OS == TargetPlatform.iOS)
				{
					MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
					{
						Title = "Camera Permission",
						Question = $"{PermissionMessage} Please go into Settings and turn on Camera for {AboutThisApp.AppName}.",
						Positive = "Settings",
						Negative = "Maybe Later",
						OnCompleted = (result) =>
							{
								if (result)
								{
									DependencyService.Get<IPushNotifications>().OpenSettings();
								}
							}
					});
					return;
				}
				else
				{
					request = true;
				}
			}

			if (request || status != PermissionStatus.Granted)
			{
				var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
				if (newStatus.ContainsKey(Permission.Camera) && newStatus[Permission.Camera] != PermissionStatus.Granted)
				{
					MessagingService.Current.SendMessage(MessageKeys.Question, new MessagingServiceQuestion
					{
						Title = "Camera Permission",
						Question = PermissionMessage,
						Positive = "Settings",
						Negative = "Maybe Later",
						OnCompleted = (result) =>
							{
								if (result)
								{
									DependencyService.Get<IPushNotifications>().OpenSettings();
								}
							}
					});
					return;
				}
			}

			await Navigation.PushAsync(scanPage);
		}

		void ScanPage_OnScanResult(ZXing.Result result)
		{
			// Stop scanning
			scanPage.IsScanning = false;
			// Pop the page and show the result
			Device.BeginInvokeOnMainThread(async () =>
			{
				await Navigation.PopAsync();

				Guid newUserId;
				if (Guid.TryParse(result.Text, out newUserId))
				{
					await vm.SyncWebToMobileAsync(newUserId.ToString());

					App.Logger.Track("SyncWebToMobile");
					await DisplayAlert("Done", "Favorites and feedback from your website account are now available in this app", "OK");
				}
				else
				{
					await DisplayAlert("Link Issue", $"That doesn't seem to be a valid user ID. Please go to the {EventInfo.EventName} website and activate the link code to scan it.", "OK");
				}

			});
		}
	}
}

