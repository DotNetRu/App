using Plugin.Connectivity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using FormsToolkit;
using Newtonsoft.Json;
using XamarinEvolve.Utils;
using XamarinEvolve.Utils.Base36;

namespace XamarinEvolve.Clients.Portable
{
    public class MiniHackDetailsViewModel : ViewModelBase
    {
        public MiniHack Hack { get; set;}
		bool queued = false;

        public MiniHackDetailsViewModel(MiniHack hack)
        {
            Hack = hack;

			MessagingService.Current.Subscribe(MessageKeys.LoggedIn, async (s) =>
				{
					if (!queued)
						return;

					await FinishHack();
				});
		}

        public async Task FinishHack ()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                MessagingUtils.SendAlert("Offline", "You are currently offline. Please go online in order to unlock this Mini-Hack.");
                return;
            }

			if (!Settings.Current.IsLoggedIn)
			{
				queued = true;
				MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
				return;
			}

			if (IsBusy)
                return;

            IsBusy = true;
            queued = false;

            try
            {
                Logger.Track(EvolveLoggerKeys.FinishMiniHack, "Name", Hack.Name);

                var mobileClient = DataStore.Azure.StoreManager.MobileService;
                if (mobileClient == null)
                    return;

                var body = JsonConvert.SerializeObject(Hack.Id);
                await mobileClient.InvokeApiAsync("CompleteMiniHack", body, HttpMethod.Post, null);

                Hack.IsCompleted = true;
                Settings.Current.FinishHack(Hack.Id);
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

		public bool AnonymousUser => !FeatureFlags.LoginEnabled;
		public string UserIdShortCode => new Base36(Settings.Current.UserIdentifier.GetHashCode()).ToString();
		public string FinishedInstruction => $"When you are finished with the Mini-Hack please see a {EventInfo.MiniHackStaffMemberName} to scan the unlock code.";
    }
}

