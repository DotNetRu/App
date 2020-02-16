namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.Clients.Portable.Extensions;
    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;
    using DotNetRu.Utils.Interfaces;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    using MenuItem = Model.MenuItem;

    public class TalkViewModel : ViewModelBase
    {
        private TalkModel talkModel;

        private ICommand shareCommand;

        private ICommand reminderCommand;

        private bool isReminderSet;

        public TalkModel TalkModel
        {
            get => this.talkModel;
            set => this.SetProperty(ref this.talkModel, value);
        }

        public ObservableRangeCollection<LocalizableMenuItem> SessionMaterialItems { get; } =
            new ObservableRangeCollection<LocalizableMenuItem>();

        public TalkViewModel(INavigation navigation, TalkModel talkModel)
            : base(navigation)
        {
            this.TalkModel = talkModel;

            this.ShowReminder = this.TalkModel.Sessions.First().EndTime.ToUniversalTime() <= DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(talkModel.PresentationUrl))
            {
                this.SessionMaterialItems.Add(
                    new LocalizableMenuItem
                        {
                            ResourceName = "PresentationSlides",
                            Parameter = talkModel.PresentationUrl,
                            Icon = "icon_presentation.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(talkModel.VideoUrl))
            {
                this.SessionMaterialItems.Add(
                    new LocalizableMenuItem
                        {
                            ResourceName = "TalkRecording",
                            Parameter = talkModel.VideoUrl,
                            Icon = "icon_video.png"
                        });
            }

            if (!string.IsNullOrWhiteSpace(talkModel.CodeUrl))
            {
                this.SessionMaterialItems.Add(
                    new LocalizableMenuItem
                        {
                            ResourceName = "CodeFromSession",
                            Parameter = talkModel.CodeUrl,
                            Icon = "icon_code.png"
                        });
            }

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.SessionMaterialItems.ForEach(x => x.Update()));
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
                if (this.selectedSessionMaterialItem == null)
                {
                    return;
                }

                this.LaunchBrowserCommand.Execute(new Uri(this.selectedSessionMaterialItem.Parameter));

                this.SelectedSessionMaterialItem = null;
            }
        }

        public bool ShowReminder { get; set; }

        public bool IsReminderSet
        {
            get => this.isReminderSet;

            set => this.SetProperty(ref this.isReminderSet, value);
        }

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

        public ICommand ShareCommand => this.shareCommand
                                        ?? (this.shareCommand = new Command(
                                                async () => await this.ExecuteShareCommandAsync()));

        private async Task ExecuteShareCommandAsync()
        {
            this.Logger.Track(DotNetRuLoggerKeys.Share, "Title", this.TalkModel.Title);

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{this.TalkModel.Title} {EventInfo.HashTag} {this.TalkModel.VideoUrl}"
            });

            // TODO fix app links
            // var speakerHandles = this.TalkModel.SpeakerHandles;
            // if (!string.IsNullOrEmpty(speakerHandles))
            // {
            // speakerHandles = " by " + speakerHandles;
            // }
            // if (FeatureFlags.AppLinksEnabled)
            // {
            // message += " " + this.TalkModel.GetWebUrl();
            // }
        }
    }
}

