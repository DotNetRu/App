namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.DataObjects;

    using FormsToolkit;

    using MvvmHelpers;

    using Plugin.Share;
    using Plugin.Share.Abstractions;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Extensions;
    using XamarinEvolve.Utils.Helpers;

    public class TalkViewModel : ViewModelBase
    {
        private Session session;

        public Session Session
        {
            get => this.session;
            set => this.SetProperty(ref this.session, value);
        }

        public ObservableRangeCollection<MenuItem> SessionMaterialItems { get; } =
            new ObservableRangeCollection<MenuItem>();

        public TalkViewModel(INavigation navigation, Session session)
            : base(navigation)
        {
            this.Session = session;
            if (this.Session.StartTime.HasValue)
            {
                this.ShowReminder = !this.Session.StartTime.Value.IsTba()
                                    && this.Session.EndTime.Value.ToUniversalTime() <= DateTime.UtcNow;
            }
            else
            {
                this.ShowReminder = false;
            }

            if (!string.IsNullOrWhiteSpace(session.PresentationUrl))
            {
                this.SessionMaterialItems.Add(
                    new MenuItem
                        {
                            Name = "Presentation Slides",
                            Parameter = session.PresentationUrl,
                            Icon = "icon_presentation.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(session.VideoUrl))
            {
                this.SessionMaterialItems.Add(
                    new MenuItem { Name = "Talk Recording", Parameter = session.VideoUrl, Icon = "icon_video.png" });
            }

            if (!string.IsNullOrWhiteSpace(session.CodeUrl))
            {
                this.SessionMaterialItems.Add(
                    new MenuItem {Name = "Code from Session", Parameter = session.CodeUrl, Icon="icon_code.png"});
            }
        }


        public bool ShowSessionMaterials => SessionMaterialItems.Any();

        MenuItem selectedSessionMaterialItem;

        public MenuItem SelectedSessionMaterialItem
        {
            get => this.selectedSessionMaterialItem;

            set
            {
                this.selectedSessionMaterialItem = value;
                this.OnPropertyChanged();
                if (this.selectedSessionMaterialItem == null) return;

                this.LaunchBrowserCommand.Execute(this.selectedSessionMaterialItem.Parameter);

                this.SelectedSessionMaterialItem = null;
            }
        }

        public bool ShowReminder { get; set; }

        bool isReminderSet;

        public bool IsReminderSet
        {
            get => this.isReminderSet;

            set => this.SetProperty(ref this.isReminderSet, value);
        }



        Speaker selectedSpeaker;

        public Speaker SelectedSpeaker
        {
            get => this.selectedSpeaker;

            set
            {
                this.selectedSpeaker = value;
                this.OnPropertyChanged();
                if (this.selectedSpeaker == null) return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSpeaker, this.selectedSpeaker);

                this.SelectedSpeaker = null;
            }
        }

        ICommand reminderCommand;

        public ICommand ReminderCommand => this.reminderCommand
                                           ?? (this.reminderCommand = new Command(
                                                   async () => await this.ExecuteReminderCommandAsync()));

        async Task ExecuteReminderCommandAsync()
        {
            if (!this.IsReminderSet)
            {
                var result = await ReminderService.AddReminderAsync(
                                 this.Session.Id,
                                 new Plugin.Calendars.Abstractions.CalendarEvent
                                     {
                                         AllDay = false,
                                         Description = this.Session.Abstract,
                                         Location = this.Session.Room?.Name
                                             ?? string.Empty,
                                         Name = this.Session.Title,
                                         Start = this.Session.StartTime.Value,
                                         End = this.Session.EndTime.Value
                                     });


                if (!result)
                {
                    return;
                }

                this.Logger.Track(EvolveLoggerKeys.ReminderAdded, "Title", this.Session.Title);
                this.IsReminderSet = true;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync(this.Session.Id);
                if (!result) return;
                this.Logger.Track(EvolveLoggerKeys.ReminderRemoved, "Title", this.Session.Title);
                this.IsReminderSet = false;
            }
        }

        ICommand shareCommand;

        public ICommand ShareCommand => this.shareCommand ?? (this.shareCommand = new Command(async () => await this.ExecuteShareCommandAsync()));

        async Task ExecuteShareCommandAsync()
        {
            this.Logger.Track(EvolveLoggerKeys.Share, "Title", this.Session.Title);
            var speakerHandles = this.Session.SpeakerHandles;
            if (!string.IsNullOrEmpty(speakerHandles))
            {
                speakerHandles = " by " + speakerHandles;
            }

            var message = $"Can't wait for {this.Session.Title}{speakerHandles} at {EventInfo.HashTag}!";

            if (FeatureFlags.AppLinksEnabled)
            {
                message += " " + this.Session.GetWebUrl();
            }

            await CrossShare.Current.Share(new ShareMessage { Text = message });
        }

        ICommand loadSessionCommand;

        public ICommand LoadSessionCommand => this.loadSessionCommand
                                              ?? (this.loadSessionCommand = new Command(
                                                      async () => await this.ExecuteLoadSessionCommandAsync()));

        public async Task ExecuteLoadSessionCommandAsync()
        {

            if (this.IsBusy) return;

            try
            {
                this.IsBusy = true;

                this.IsReminderSet = await ReminderService.HasReminderAsync(this.Session.Id);
                this.Session.FeedbackLeft = await this.StoreManager.FeedbackStore.LeftFeedback(this.Session);


            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadSessionCommandAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}

