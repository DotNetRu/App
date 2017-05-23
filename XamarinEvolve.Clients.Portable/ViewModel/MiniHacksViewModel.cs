using System;
using MvvmHelpers;
using XamarinEvolve.DataObjects;
using System.Windows.Input;
using System.Threading.Tasks;
using FormsToolkit;
using Xamarin.Forms;
using System.Linq;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
    public class MiniHacksViewModel : ViewModelBase
    {
        public MiniHacksViewModel()
        {
        }

        public ObservableRangeCollection<MiniHack> MiniHacks { get; } = new ObservableRangeCollection<MiniHack>();

        bool noHacksFound;
        public bool NoHacksFound
        {
            get { return noHacksFound; }
            set { SetProperty(ref noHacksFound, value); }
        }

		public string NoHacksText => $"Mini-Hacks will be revealed at {EventInfo.EventName}. Check back soon.";

        #region Commands


        ICommand  forceRefreshCommand;
        public ICommand ForceRefreshCommand =>
            forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync())); 

        async Task ExecuteForceRefreshCommandAsync()
        {
            await ExecuteLoadMiniHacksAsync(true);
        }



        ICommand loadMiniHacksCommand;
        public ICommand LoadMiniHacksCommand =>
        loadMiniHacksCommand ?? (loadMiniHacksCommand = new Command<bool>(async (f) => await ExecuteLoadMiniHacksAsync())); 

        async Task<bool> ExecuteLoadMiniHacksAsync(bool force = false)
        {
            if(IsBusy)
                return false;

            try 
            {
                IsBusy = true;
                NoHacksFound = false;

                #if DEBUG
                await Task.Delay(1000);
                #endif

                var hacks = await StoreManager.MiniHacksStore.GetItemsAsync(force);
                var finalHacks = hacks.ToList ();
				foreach (var hack in finalHacks)
				{
					try
					{
						if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows && FeatureFlags.AppLinksEnabled)
						{
							Application.Current.AppLinks.RegisterLink(hack.GetAppLink());
						}
					}
					catch (Exception applinkException)
					{
						// don't crash the app
						Logger.Report(applinkException, "AppLinks.RegisterLink", hack.Id);
					}

					hack.IsCompleted = Settings.Current.IsHackFinished(hack.Id);
				}

                MiniHacks.ReplaceRange(finalHacks);

                NoHacksFound = MiniHacks.Count == 0;

            } 
            catch (Exception ex) 
            {
                Logger.Report(ex, "Method", "ExecuteLoadMiniHacksAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }


        #endregion
    }
}

