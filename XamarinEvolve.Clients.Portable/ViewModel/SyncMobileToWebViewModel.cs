using System;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.ComponentModel;
using Plugin.Connectivity;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	public class SyncMobileToWebViewModel : ViewModelBase
	{
		public SyncMobileToWebViewModel(INavigation navigation) : base(navigation)
		{
			PropertyChanged += MonitorBusy;
		}

		void MonitorBusy(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IsBusy))
			{
				OnPropertyChanged(nameof(GetSyncCodeButtonAvailable));
			}
		}

		string syncCode;
		public string SyncCode
		{
			get { return syncCode; }
			set { SetProperty(ref syncCode, value); }
		}

		bool syncCodeError;
		public bool SyncCodeError
		{
			get { return syncCodeError; }
			set { SetProperty(ref syncCodeError, value); }
		}

		bool resultVisible;
		public bool ResultVisible
		{
			get { return resultVisible; }
			set 
			{ 
				SetProperty(ref resultVisible, value);
				OnPropertyChanged(nameof(GetSyncCodeButtonAvailable));
			}
		}

		public string Explanation => $"The {EventInfo.EventName} app stores your favorites under an anonymous account." +
		$"You can link the {EventInfo.EventName} website to this same account with this manual step.";

		public string Explanation2 => $"By clicking 'Get Temporary Code' below, you will receive a temporary code which you can enter in the {EventInfo.EventName} website. " +
		"The website will then link to this account and you will see the same favorites and session feedback there.";

		public string ResultText => $"Enter this code in the {EventInfo.EventName} website to link.";

		ICommand getSyncCodeCommand;
		public ICommand GetSyncCodeCommand =>
			getSyncCodeCommand ?? (getSyncCodeCommand = new Command(async () => await ExecuteGetSyncCodeAsync()));

		public bool GetSyncCodeButtonAvailable => !IsBusy && !ResultVisible;

		ICommand doneCommand;
		public ICommand DoneCommand =>
			doneCommand ?? (doneCommand = new Command(async () => await Navigation.PopAsync(true)));

		async Task ExecuteGetSyncCodeAsync()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				MessagingUtils.SendOfflineMessage();
				return;
			}

			if (IsBusy)
				return;

			IsBusy = true;
			SyncCodeError = false;
			try
			{				
				Logger.Track(EvolveLoggerKeys.GetSyncCode);

				var mobileClient = DataStore.Azure.StoreManager.MobileService;
				if (mobileClient == null)
					return;

				var code = new MobileToWebSync();
				var result = await mobileClient.InvokeApiAsync<MobileToWebSync, MobileToWebSync>("MobileToWebSync", code, HttpMethod.Post, null);

				if (result == null)
				{
					SyncCodeError = true;
					return;
				}

				SyncCode = result.TempCode;
				ResultVisible = true;
			}
			catch (Exception ex)
			{
				SyncCodeError = true;
				Logger.Report(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}

