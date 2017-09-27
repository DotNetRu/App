using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using DotNetRu.DataStore.Audit.DataObjects;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;
using XamarinEvolve.Utils;
using XamarinEvolve.Utils.Helpers;
using AuditSpeaker = DotNetRu.DataStore.Audit.Models.Speaker;

namespace XamarinEvolve.Clients.Portable
{
    public class SpeakersViewModel : ViewModelBase
    {
        private List<Speaker> _speakers; // use Speakers instead of _speakers

        public SpeakersViewModel(INavigation navigation) : base(navigation)
        {
        }

        public ObservableRangeCollection<Speaker> Speakers { get; } = new ObservableRangeCollection<Speaker>();

        #region Sorting

        private void SortSpeakers(IEnumerable<Speaker> speakers)
        {
            var speakersSorted = from speaker in speakers
                                 orderby speaker.FullName
                                 select speaker;

            Speakers.ReplaceRange(speakersSorted);
        }

        #endregion

        #region Properties

        private AuditSpeaker selectedSpeaker; // Q: MB wrong speaker type?

        public AuditSpeaker SelectedSpeaker
        {
            get => selectedSpeaker;
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

        #region Commands

        private ICommand forceRefreshCommand;

        public ICommand ForceRefreshCommand =>
            forceRefreshCommand ?? (forceRefreshCommand =
                new Command(async () => await ExecuteForceRefreshCommandAsync()));

        private async Task ExecuteForceRefreshCommandAsync()
        {
            await ExecuteLoadSpeakersAsync(true);
        }

        private ICommand loadSpeakersCommand;

        public ICommand LoadSpeakersCommand =>
            loadSpeakersCommand ?? (loadSpeakersCommand =
                new Command(async f => await ExecuteLoadSpeakersAsync((bool)f)));

        private List<Speaker> AuditSpeakerToUISpeakerConverter(IEnumerable<AuditSpeaker> auditSpeakers)
        {
            return auditSpeakers.Select(speaker => new Speaker
            {
                FirstName = speaker.Name,
                LastName = "",
                PhotoUrl = $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{speaker.Id}/avatar.jpg",
                AvatarUrl = $@"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{speaker.Id}/avatar.small.jpg",
                CompanyName = speaker.CompanyName,
                CompanyWebsiteUrl = speaker.CompanyUrl,
                TwitterUrl = speaker.TwitterUrl,
                BlogUrl = speaker.BlogUrl,
                Biography = speaker.Description
            }).ToList();
        }

        private List<Speaker> GetSpeakers()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.speakers.xml");
            List<AuditSpeaker> speakers;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute
                {
                    ElementName = "Speakers",
                    IsNullable = true
                };
                var serializer = new XmlSerializer(typeof(List<AuditSpeaker>), xRoot);
                speakers = (List<AuditSpeaker>)serializer.Deserialize(reader);
            }
            return AuditSpeakerToUISpeakerConverter(speakers);
        }

        private async Task<bool> ExecuteLoadSpeakersAsync(bool force = false)
        {
            if (IsBusy)
                return false;

            try
            {
                IsBusy = true;

#if DEBUG
                await Task.Delay(1000);
#endif
                if (_speakers == null) // TODO: update data when we'll have finally managed to get them directly from github
                {
                    IEnumerable<Speaker>
                        speakers = GetSpeakers(); //await StoreManager.SpeakerStore.GetItemsAsync(force);

                    SortSpeakers(speakers);
                    _speakers = speakers.ToList();
                }


                if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows &&
                    FeatureFlags.AppLinksEnabled)
                    foreach (var speaker in Speakers)
                        try
                        {
                            // data migration: older applinks are removed so the index is rebuilt again
                            Application.Current.AppLinks.DeregisterLink(new Uri(
                                $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory}/{speaker.Id}"));

                            Application.Current.AppLinks.RegisterLink(speaker.GetAppLink());
                        }
                        catch (Exception applinkException)
                        {
                            // don't crash the app
                            Logger.Report(applinkException, "AppLinks.RegisterLink", speaker.Id);
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