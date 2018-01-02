using System;
using System.Windows.Input;
using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.Portable.Model.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.Services;
using DotNetRu.Utils.Helpers;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;

namespace DotNetRu.Clients.Portable.ViewModel
{
    /// <inheritdoc />
    public class MeetupsViewModel : ViewModelBase
    {
        private MeetupModel selectedMeetup;

        /// <summary>
        /// The load events command.
        /// </summary>
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
            this.MeetupsByMonth.ReplaceRange(MeetupService.Meetups.GroupByMonth());
        }

        /// <summary>
        /// The execute load events async.
        /// </summary>
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
                this.Logger.Report(ex, "Method", "ExecuteLoadMeetups");
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