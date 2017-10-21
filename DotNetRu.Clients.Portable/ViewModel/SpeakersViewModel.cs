namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.DataObjects;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils.Helpers;

    public class SpeakersViewModel : ViewModelBase
    {

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

            this.Speakers.ReplaceRange(speakersSorted);
        }

        #endregion

        #region Properties

        private Speaker _selectedSpeaker;

        public Speaker SelectedSpeaker
        {
            get => this._selectedSpeaker;
            set
            {
                this._selectedSpeaker = value;
                this.OnPropertyChanged();
                if (this._selectedSpeaker == null)
                    return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSpeaker, this._selectedSpeaker);

                this.SelectedSpeaker = null;
            }
        }

        #endregion

        #region Commands

        private ICommand loadSpeakersCommand;

        public ICommand LoadSpeakersCommand => this.loadSpeakersCommand ?? (this.loadSpeakersCommand =
                new Command(async f => await this.ExecuteLoadSpeakersAsync((bool)f)));

        private async Task<bool> ExecuteLoadSpeakersAsync(bool force = false)
        {
            if (this.IsBusy)
            {
                return false;
            }

            try
            {
                this.IsBusy = true;

                // TODO: update data when we'll have finally managed to get them directly from github
                if (!this.Speakers.Any() || force) 
                {
                    IEnumerable<Speaker> speakers = SpeakerLoaderService.Speakers;
                    this.SortSpeakers(speakers);
                }

                // TODO uncomment
                //if (Device.RuntimePlatform != Device.UWP && FeatureFlags.AppLinksEnabled)
                //{
                //    foreach (var speaker in this.Speakers)
                //    {
                //        try
                //        {
                //            // data migration: older applinks are removed so the index is rebuilt again
                //            Application.Current.AppLinks.DeregisterLink(
                //                new Uri(
                //                    $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SpeakersSiteSubdirectory}/{speaker.Id}"));

                //            Application.Current.AppLinks.RegisterLink(speaker.GetAppLink());
                //        }
                //        catch (Exception applinkException)
                //        {
                //            // don't crash the app
                //            this.Logger.Report(applinkException, "AppLinks.RegisterLink", speaker.Id);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadSpeakersAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return true;
        }

        #endregion
    }
}