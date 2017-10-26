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
        private TalkModel talkModel;

        public TalkModel TalkModel
        {
            get => this.talkModel;
            set => this.SetProperty(ref this.talkModel, value);
        }

        public ObservableRangeCollection<MenuItem> SessionMaterialItems { get; } =
            new ObservableRangeCollection<MenuItem>();

        public TalkViewModel(INavigation navigation, TalkModel talkModel)
            : base(navigation)
        {
            this.TalkModel = talkModel;
            if (this.TalkModel.StartTime.HasValue)
            {
                this.ShowReminder = !this.TalkModel.StartTime.Value.IsTba()
                                    && this.TalkModel.EndTime.Value.ToUniversalTime() <= DateTime.UtcNow;
            }
            else
            {
                this.ShowReminder = false;
            }

            if (!string.IsNullOrWhiteSpace(talkModel.PresentationUrl))
            {
                this.SessionMaterialItems.Add(
                    new MenuItem
                        {
                            Name = "Presentation Slides",
                            Parameter = talkModel.PresentationUrl,
                            Icon = "icon_presentation.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(talkModel.VideoUrl))
            {
                this.SessionMaterialItems.Add(
                    new MenuItem { Name = "Talk Recording", Parameter = talkModel.VideoUrl, Icon = "icon_video.png" });
            }

            if (!string.IsNullOrWhiteSpace(talkModel.CodeUrl))
            {
                this.SessionMaterialItems.Add(
                    new MenuItem {Name = "Code from Session", Parameter = talkModel.CodeUrl, Icon="icon_code.png"});
            }
        }


        public bool ShowSessionMaterials => this.SessionMaterialItems.Any();

        private MenuItem selectedSessionMaterialItem;

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

        private bool isReminderSet;

        public bool IsReminderSet
        {
            get => this.isReminderSet;

            set => this.SetProperty(ref this.isReminderSet, value);
        }

        private Speaker selectedSpeaker;

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

        private ICommand reminderCommand;

        public ICommand ReminderCommand => this.reminderCommand
                                           ?? (this.reminderCommand = new Command(
                                                   async () => await this.ExecuteReminderCommandAsync()));

        async Task ExecuteReminderCommandAsync()
        {
            if (!this.IsReminderSet)
            {
                var result = await ReminderService.AddReminderAsync(
                                 this.TalkModel.Id,
                                 new Plugin.Calendars.Abstractions.CalendarEvent
                                     {
                                         AllDay = false,
                                         Description = this.TalkModel.Abstract,
                                         Location = this.TalkModel.Room?.Name
                                             ?? string.Empty,
                                         Name = this.TalkModel.Title,
                                         Start = this.TalkModel.StartTime.Value,
                                         End = this.TalkModel.EndTime.Value
                                     });


                if (!result)
                {
                    return;
                }

                this.Logger.Track(EvolveLoggerKeys.ReminderAdded, "Title", this.TalkModel.Title);
                this.IsReminderSet = true;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync(this.TalkModel.Id);
                if (!result) return;
                this.Logger.Track(EvolveLoggerKeys.ReminderRemoved, "Title", this.TalkModel.Title);
                this.IsReminderSet = false;
            }
        }

        private ICommand shareCommand;

        public ICommand ShareCommand => this.shareCommand
                                        ?? (this.shareCommand = new Command(
                                                async () => await this.ExecuteShareCommandAsync()));

        public async Task ExecuteShareCommandAsync()
        {
            this.Logger.Track(EvolveLoggerKeys.Share, "Title", this.TalkModel.Title);
            var speakerHandles = this.TalkModel.SpeakerHandles;
            if (!string.IsNullOrEmpty(speakerHandles))
            {
                speakerHandles = " by " + speakerHandles;
            }

            var message = $"Can't wait for {this.TalkModel.Title}{speakerHandles} at {EventInfo.HashTag}!";

            if (FeatureFlags.AppLinksEnabled)
            {
                message += " " + this.TalkModel.GetWebUrl();
            }

            await CrossShare.Current.Share(new ShareMessage { Text = message });
        }

        private ICommand loadSessionCommand;

        public ICommand LoadSessionCommand => this.loadSessionCommand
                                              ?? (this.loadSessionCommand = new Command(
                                                      async () => await this.ExecuteLoadSessionCommandAsync()));

        public async Task ExecuteLoadSessionCommandAsync()
        {

            if (this.IsBusy) return;

            try
            {
                this.IsBusy = true;

                this.IsReminderSet = await ReminderService.HasReminderAsync(this.TalkModel.Id);
                this.TalkModel.FeedbackLeft = await this.StoreManager.FeedbackStore.LeftFeedback(this.TalkModel);


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

