using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using System.Windows.Input;
using System.Threading.Tasks;
using Plugin.EmbeddedResource;
using XamarinEvolve.Clients.Portable.Helpers;
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

	    private IEnumerable<Speaker> GetSpeakers()
	    {
	        var xml = ResourceLoader.GetEmbeddedResourceString(Assembly.Load(new AssemblyName("XamarinEvolve.Clients.Portable")), "index.xml");
	        var x = xml.ParseXml<DotNetRu.DataStore.Audit.Speaker>();
            return  new List<Speaker>
            {
                new Speaker
                {
                    FirstName = x.Name,
                    LastName = "",
                    PhotoUrl = @"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/Pavel-Fedotovsky/avatar.jpg",
                    AvatarUrl = @"https://rawgit.com/DotNetRu/Audit/master/db/speakers/Pavel-Fedotovsky/avatar.small.jpg",
                    CompanyName = x.CompanyName,
                    CompanyWebsiteUrl = x.CompanyUrl,
                    Biography = x.Description
                }
            };
	    }

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
			    var speakers = GetSpeakers();//await StoreManager.SpeakerStore.GetItemsAsync(force);
			    var k = speakers.First().FullName;
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

