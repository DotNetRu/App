namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils.Helpers;

    public class SpeakersViewModel : ViewModelBase
    {
        public SpeakersViewModel(INavigation navigation) : base(navigation)
        {
        }

        public ObservableRangeCollection<SpeakerModel> Speakers { get; } = new ObservableRangeCollection<SpeakerModel>();
       
        #region Sorting

        private void SortSpeakers(IEnumerable<SpeakerModel> speakers)
        {
            var speakersSorted = from speaker in speakers
                                 orderby speaker.FullName
                                 select speaker;

            this.Speakers.ReplaceRange(speakersSorted);
        }

        #endregion
        
        private SpeakerModel selectedSpeakerModel;

        public SpeakerModel SelectedSpeakerModel
        {
            get => this.selectedSpeakerModel;
            set
            {
                this.selectedSpeakerModel = value;
                this.OnPropertyChanged();
                if (this.selectedSpeakerModel == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSpeaker, this.selectedSpeakerModel);

                this.SelectedSpeakerModel = null;
            }
        }

        private ICommand loadSpeakersCommand;

        public ICommand LoadSpeakersCommand => this.loadSpeakersCommand ?? (this.loadSpeakersCommand =
                new Command((f) => this.ExecuteLoadSpeakers((bool)f)));

        private void ExecuteLoadSpeakers(bool force = false)
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;

                // TODO: update data when we'll have finally managed to get them directly from github
                if (!this.Speakers.Any() || force) 
                {
                    IEnumerable<SpeakerModel> speakers = SpeakerService.Speakers;
                    this.SortSpeakers(speakers);
                }

                // TODO uncomment
                // if (Device.RuntimePlatform != Device.UWP && FeatureFlags.AppLinksEnabled)
                // {
                // foreach (var speaker in this.Speakers)
                // {
                // try
                // {
                // // data migration: older applinks are removed so the index is rebuilt again
                // Application.Current.AppLinks.DeregisterLink(
                // new Uri(
                // $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory}/{speaker.Id}"));

                // Application.Current.AppLinks.RegisterLink(speaker.GetAppLink());
                // }
                // catch (Exception applinkException)
                // {
                // // don't crash the app
                // this.Logger.Report(applinkException, "AppLinks.RegisterLink", speaker.Id);
                // }
                // }
                // }
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadSpeakers");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return;
        }
    }
}