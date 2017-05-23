using System;
using System.Linq;
using System.Collections.Generic;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using System.Windows.Input;
using System.Threading.Tasks;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	public class SpeakersViewModel: ViewModelBase
	{
		public SpeakersViewModel(INavigation navigation) : base(navigation)
        {

		}

		public ObservableRangeCollection<Speaker> Speakers { get; } = new ObservableRangeCollection<Speaker>();

		#region Properties
		Speaker selectedSpeaker;
		public Speaker SelectedSpeaker
		{
			get { return selectedSpeaker; }
			set
			{
				selectedSpeaker = value;
				OnPropertyChanged();
				if (selectedSpeaker == null)
					return;

				MessagingService.Current.SendMessage(MessageKeys.NavigateToSpeaker, selectedSpeaker);

				SelectedSpeaker = null;
			}
		}

		#endregion

		#region Sorting

		void SortSpeakers(IEnumerable<Speaker> speakers)
		{
			var speakersSorted = from speaker in speakers
								 orderby speaker.FullName
								 select speaker;

			Speakers.ReplaceRange(speakersSorted);
		}



		#endregion

		#region Commands

		ICommand forceRefreshCommand;
		public ICommand ForceRefreshCommand =>
		forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

		async Task ExecuteForceRefreshCommandAsync()
		{
			await ExecuteLoadSpeakersAsync(true);
		}

		ICommand loadSpeakersCommand;
		public ICommand LoadSpeakersCommand =>
			loadSpeakersCommand ?? (loadSpeakersCommand = new Command(async (f) => await ExecuteLoadSpeakersAsync((bool) f)));

		async Task<bool> ExecuteLoadSpeakersAsync(bool force = false)
		{
			if (IsBusy)
				return false;

			try
			{
				IsBusy = true;

#if DEBUG
				await Task.Delay(1000);
#endif
				var speakers = await StoreManager.SpeakerStore.GetItemsAsync(force);

				SortSpeakers(speakers);

				if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows && FeatureFlags.AppLinksEnabled)
				{
					foreach (var speaker in Speakers)
					{
						try
						{
							// data migration: older applinks are removed so the index is rebuilt again
							Application.Current.AppLinks.DeregisterLink(new Uri($"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory}/{speaker.Id}"));

							Application.Current.AppLinks.RegisterLink(speaker.GetAppLink());
						}
						catch (Exception applinkException)
						{
							// don't crash the app
							Logger.Report(applinkException, "AppLinks.RegisterLink", speaker.Id);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Report(ex, "Method", "ExecuteLoadSpeakersAsync");
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

