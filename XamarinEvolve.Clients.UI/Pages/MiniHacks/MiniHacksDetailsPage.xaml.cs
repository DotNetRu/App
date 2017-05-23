using System;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Clients.Portable;
using ZXing.Net.Mobile.Forms;
using ZXing.Mobile;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using FormsToolkit;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	public partial class MiniHacksDetailsPage : BasePage
	{
		public override AppPage PageType => AppPage.MiniHack;

		IPlatformSpecificExtension<MiniHack> _extension;

		MiniHackDetailsViewModel vm;
		ZXingScannerPage scanPage;
		public MiniHacksDetailsPage(MiniHack hack)
		{
			InitializeComponent();
            ItemId = hack.Name;

            _extension = DependencyService.Get<IPlatformSpecificExtension<MiniHack>>();

			BindingContext = vm = new MiniHackDetailsViewModel(hack);

			ButtonFinish.Clicked += ButtonFinish_Clicked;

			if (string.IsNullOrWhiteSpace(hack.GitHubUrl))
			{
				MiniHackDirections1.IsEnabled = false;
				MiniHackDirections1.Text = "Directions coming soon";
				MiniHackDirections2.IsEnabled = false;
				MiniHackDirections2.Text = "Directions coming soon";
			}

			scanPage = new ZXingScannerPage(new MobileBarcodeScanningOptions { AutoRotate = false, })
			{
				DefaultOverlayTopText = "Align the barcode within the frame",
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

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (_extension != null)
			{
				await _extension.Execute(vm.Hack);
			}
		}

		protected async override void OnDisappearing()
		{
			base.OnDisappearing();

			if (_extension != null)
			{
				await _extension.Finish();
			}
		}

		async void ButtonFinish_Clicked(object sender, EventArgs e)
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
						Question = $"To finish mini-hacks you will need to scan a code and the camera permission is required. Please go into Settings and turn on Camera for {AboutThisApp.AppName}.",
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
					MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
					{
						Title = "Camera Permission",
						Question = "To finish mini-hacks you will need to scan a code and the camera permission is required.",
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

				if (vm.Hack.UnlockCode == result.Text)
				{
					await vm.FinishHack();
					await DisplayAlert("Congratulations", "You successfully finished the Mini-Hack!", "OK");
				}
				else
				{
					await DisplayAlert("Mini-Hack Issue", $"That doesn't seem to be the right code. Please see a {EventInfo.MiniHackStaffMemberName} to help finish the Mini-Hack.", "OK");
				}
			});
		}
	}
}

