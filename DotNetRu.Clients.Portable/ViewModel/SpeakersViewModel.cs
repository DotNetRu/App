namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    public class SpeakersViewModel : ViewModelBase
    {
        public SpeakersViewModel(INavigation navigation)
            : base(navigation)
        {
        }

        public ObservableRangeCollection<Grouping<char, SpeakerModel>> speakers = new ObservableRangeCollection<Grouping<char, SpeakerModel>>();
        public ObservableCollection<Grouping<char, SpeakerModel>> Speakers => speakers;

        #region Sorting

        private void SortSpeakers(IEnumerable<SpeakerModel> speakers)
        {
            var speakersSorted = from speaker in speakers 
                                    orderby speaker.FirstName 
                                    group speaker by Char.ToUpperInvariant(speaker.FirstName[0]) into speakerGroup
                                    select new Grouping<char, SpeakerModel>(speakerGroup.Key, speakerGroup);

            this.speakers.ReplaceRange(speakersSorted);
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

        public ICommand LoadSpeakersCommand =>
            this.loadSpeakersCommand
            ?? (this.loadSpeakersCommand = new Command((f) => this.ExecuteLoadSpeakers((bool)f)));

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