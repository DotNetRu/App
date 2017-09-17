using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;

using System.Windows.Input;
using System.Threading.Tasks;

using XamarinEvolve.Clients.Portable.Helpers;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.Models;

    using XamarinEvolve.Utils.Helpers;

	public class SpeakersViewModel: ViewModelBase
	{
		public SpeakersViewModel(INavigation navigation) : base(navigation)
        {

		}

		public ObservableRangeCollection<DotNetRu.DataStore.Audit.DataObjects.Speaker> Speakers { get; } = new ObservableRangeCollection<DotNetRu.DataStore.Audit.DataObjects.Speaker>();

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

		void SortSpeakers(IEnumerable<DotNetRu.DataStore.Audit.DataObjects.Speaker> speakers)
		{
			IOrderedEnumerable<DotNetRu.DataStore.Audit.DataObjects.Speaker> speakersSorted = from speaker in speakers
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

	    private static IEnumerable<DotNetRu.DataStore.Audit.DataObjects.Speaker> GetSpeakers()
	    {
	        var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
	        const string resourceFileName = "index.xml";

            var resourceNames = assembly.GetManifestResourceNames();
	        var resourcePaths = resourceNames
	            .Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
	            .ToArray();

	        var speakers = new List<DotNetRu.DataStore.Audit.DataObjects.Speaker>();
	        foreach (var resource in resourcePaths)
	        {
	            var stream = assembly.GetManifestResourceStream(resource);
	            using (var streamReader = new StreamReader(stream))
	            {
	                var xml = streamReader.ReadToEnd();
	                // ReSharper disable once InconsistentNaming
	                Speaker DotNEXTspeaker = xml.ParseXml<DotNetRu.DataStore.Audit.Models.Speaker>();
                    speakers.Add(new DotNetRu.DataStore.Audit.DataObjects.Speaker
                    {
                        FirstName = DotNEXTspeaker.Name,
                        LastName = "",
                        PhotoUrl = $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{DotNEXTspeaker.Id}/avatar.jpg",
                        AvatarUrl = $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{DotNEXTspeaker.Id}/avatar.small.jpg",
                        CompanyName = DotNEXTspeaker.CompanyName,
                        CompanyWebsiteUrl = DotNEXTspeaker.CompanyUrl,
                        TwitterUrl = DotNEXTspeaker.TwitterUrl,
                        BlogUrl = DotNEXTspeaker.BlogUrl,
                        Biography = DotNEXTspeaker.Description
                    });
                }

            }
	        return speakers;
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
			    IEnumerable<DotNetRu.DataStore.Audit.DataObjects.Speaker> speakers = GetSpeakers();//await StoreManager.SpeakerStore.GetItemsAsync(force);
                
				SortSpeakers(speakers);

				if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows && FeatureFlags.AppLinksEnabled)
				{
					foreach (DotNetRu.DataStore.Audit.DataObjects.Speaker speaker in Speakers)
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

