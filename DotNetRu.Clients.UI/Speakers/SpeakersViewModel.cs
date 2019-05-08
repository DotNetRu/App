namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;
    using DotNetRu.Utils;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    public class SpeakersViewModel : ViewModelBase
    {
        public SpeakersViewModel(INavigation navigation)
            : base(navigation)
        {
            MessagingCenter.Subscribe<AuditRefresher>(this, MessageKeys.SpeakersChanged, sender => this.UpdateSpeakers());
        }

        public ObservableRangeCollection<Grouping<char, SpeakerModel>> speakers = new ObservableRangeCollection<Grouping<char, SpeakerModel>>();
        public ObservableCollection<Grouping<char, SpeakerModel>> Speakers => speakers;

        #region Sorting

        private void SortSpeakers(IEnumerable<SpeakerModel> speakers)
        {            
            var speakersSorted = speakers.Select(speaker => new { speaker, firstChar = Char.ToUpperInvariant(speaker.FirstName[0])})
                                         .OrderBy(s => 'A' <= s.firstChar && s.firstChar <= 'Z') //english names in the bottom
                                         .ThenBy(s => s.speaker.FirstName)
                                         .GroupBy(s => s.firstChar)
                                         .Select(g => new Grouping<char, SpeakerModel>(g.Key, g.Select(s => s.speaker)));

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
                    UpdateSpeakers();
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
                DotNetRuLogger.Report(ex);
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return;
        }

        private void UpdateSpeakers()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IEnumerable<SpeakerModel> speakers = RealmService.Get<SpeakerModel>();
                this.SortSpeakers(speakers);
            });
        }
    }
}
