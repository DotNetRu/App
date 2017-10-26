namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils.Helpers;

    public class ConferenceFeedbackViewModel : ViewModelBase
    {
        public ConferenceFeedbackViewModel(INavigation navigation)
            : base(navigation)
        {
            this.Title = "Conference Feedback";
        }

        private int _question1;

        public int Question1
        {
            get => this._question1;
            set => this.SetProperty(ref this._question1, value);
        }

        private int _question2;

        public int Question2
        {
            get => this._question2;
            set => this.SetProperty(ref this._question2, value);
        }

        private int _question3;

        public int Question3
        {
            get => this._question3;
            set => this.SetProperty(ref this._question3, value);
        }

        private int _question4;

        public int Question4
        {
            get => this._question4;

            set => this.SetProperty(ref this._question4, value);
        }

        private int _question5;

        public int Question5
        {
            get => this._question5;

            set => this.SetProperty(ref this._question5, value);
        }

        private int _question6;

        public int Question6
        {
            get => this._question6;

            set => this.SetProperty(ref this._question6, value);
        }

        private int _question7;

        public int Question7
        {
            get => this._question7;

            set => this.SetProperty(ref this._question7, value);
        }

        private int _question8;

        public int Question8
        {
            get => this._question8;

            set => this.SetProperty(ref this._question8, value);
        }

        private int _question9;

        public int Question9
        {
            get => this._question9;

            set => this.SetProperty(ref this._question9, value);
        }

        private int _question10;

        public int Question10
        {
            get => this._question10;

            set => this.SetProperty(ref this._question10, value);
        }

        ICommand submitFeedbackCommand;

        public ICommand SubmitFeedbackCommand => this.submitFeedbackCommand
                                                 ?? (this.submitFeedbackCommand = new Command(
                                                         async () => await this.ExecuteSubmitFeedbackCommandAsync()));

        async Task ExecuteSubmitFeedbackCommandAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            try
            {
                if (this.Question1 == 0 || this.Question2 == 0 || this.Question3 == 0 || this.Question4 == 0 || this.Question5 == 0
                )
                {
                    // 6 and 7 can be empty
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(
                        MessageKeys.Message,
                        new MessagingServiceAlert
                            {
                                Title = "Missing answers",
                                Message = "Please provide a rating for all questions.",
                                Cancel = "OK"
                            });
                    return;
                }

                this.Logger.Track(EvolveLoggerKeys.ConferenceFeedback);

                MessagingService.Current.SendMessage<MessagingServiceAlert>(
                    MessageKeys.Message,
                    new MessagingServiceAlert
                        {
                            Title = "Feedback Received",
                            Message =
                                $"Thanks for the feedback!",
                            Cancel = "OK",
                            OnCompleted = async () =>
                                {
                                    await this.Navigation.PopModalAsync();
                                    MessagingService.Current.SendMessage(
                                        "conferencefeedback_finished");
                                }
                        });

                var versionProvider = DependencyService.Get<IAppVersionProvider>();

                var feedback = new ConferenceFeedback
                                   {
                                       Question1 = this.Question1,
                                       Question2 = this.Question2,
                                       Question3 = this.Question3,
                                       Question4 = this.Question4,
                                       Question5 = this.Question5,
                                       Question6 = this.Question6,
                                       Question7 = this.Question7,
                                       Question8 = this.Question8,
                                       Question9 = this.Question9,
                                       Question10 = this.Question10,
                                       DeviceOS = Device.RuntimePlatform,
                                       AppVersion = versionProvider.AppVersion,
                                   };

                await this.StoreManager.ConferenceFeedbackStore.InsertAsync(feedback);

                Settings.Current.LeaveConferenceFeedback();
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
