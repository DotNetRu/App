namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Windows.Input;
    using DotNetRu.AppUtils;
    using DotNetRu.AppUtils.Logging;
    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Model.Extensions;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    using FormsToolkit;

    using MvvmHelpers;

    using Microsoft.Maui;

    public class MeetupsViewModel : ViewModelBase
    {
        private MeetupModel selectedMeetup;

        private ICommand loadMeetupsCommand;

        public MeetupsViewModel(INavigation navigation)
            : base(navigation)
        {
            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => this.UpdateMeetups());
            this.Title = "Meetups";
        }

        public ICommand LoadMeetupsCommand => this.loadMeetupsCommand
                                             ?? (this.loadMeetupsCommand = new Command(
                                                     () => this.ExecuteLoadMeetups()));

        public ObservableRangeCollection<Grouping<string, MeetupModel>> MeetupsByMonth { get; } =
            new ObservableRangeCollection<Grouping<string, MeetupModel>>();

        public MeetupModel SelectedMeetup
        {
            get => this.selectedMeetup;
            set
            {
                this.selectedMeetup = value;
                this.OnPropertyChanged();
                if (this.selectedMeetup == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToEvent, this.selectedMeetup);

                this.SelectedMeetup = null;
            }
        }

        public void UpdateMeetups()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.MeetupsByMonth.ReplaceRange(RealmService.Get<MeetupModel>().GroupByMonth());
            });
        }

        public bool ExecuteLoadMeetups()
        {
            if (this.IsBusy)
            {
                return false;
            }

            try
            {
                this.IsBusy = true;
                this.UpdateMeetups();
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

            return true;
        }
    }
}
