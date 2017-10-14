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
            Title = "Conference Feedback";
        }

        private int _question1;

        public int Question1
        {
            get => _question1;
            set => SetProperty(ref _question1, value);
        }

        private int _question2;

        public int Question2
        {
            get => _question2;
            set => this.SetProperty(ref _question2, value);
        }

        private int _question3;

        public int Question3
        {
            get => _question3;
            set => SetProperty(ref _question3, value);
        }

        private int _question4;

        public int Question4
        {
            get
            {
                return _question4;
            }
            set
            {
                SetProperty(ref _question4, value);
            }
        }

        private int _question5;

        public int Question5
        {
            get
            {
                return _question5;
            }
            set
            {
                SetProperty(ref _question5, value);
            }
        }

        private int _question6;

        public int Question6
        {
            get
            {
                return _question6;
            }
            set
            {
                SetProperty(ref _question6, value);
            }
        }

        private int _question7;

        public int Question7
        {
            get
            {
                return _question7;
            }
            set
            {
                SetProperty(ref _question7, value);
            }
        }

        private int _question8;

        public int Question8
        {
            get
            {
                return _question8;
            }
            set
            {
                SetProperty(ref _question8, value);
            }
        }

        private int _question9;

        public int Question9
        {
            get
            {
                return _question9;
            }
            set
            {
                SetProperty(ref _question9, value);
            }
        }

        private int _question10;

        public int Question10
        {
            get
            {
                return _question10;
            }
            set
            {
                SetProperty(ref _question10, value);
            }
        }

        ICommand submitFeedbackCommand;

        public ICommand SubmitFeedbackCommand => submitFeedbackCommand
                                                 ?? (submitFeedbackCommand = new Command(
                                                         async () => await ExecuteSubmitFeedbackCommandAsync()));

        async Task ExecuteSubmitFeedbackCommandAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (Question1 == 0 || Question2 == 0 || Question3 == 0 || Question4 == 0 || Question5 == 0
                ) // 6 and 7 can be empty
                {
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

                Logger.Track(EvolveLoggerKeys.ConferenceFeedback);

                MessagingService.Current.SendMessage<MessagingServiceAlert>(
                    MessageKeys.Message,
                    new MessagingServiceAlert
                        {
                            Title = "Feedback Received",
                            Message =
                                $"Thanks for the feedback, we hope you had a great {EventInfo.EventName}.",
                            Cancel = "OK",
                            OnCompleted = async () =>
                                {
                                    await Navigation.PopModalAsync();
                                    MessagingService.Current.SendMessage(
                                        "conferencefeedback_finished");
                                }
                        });

                var versionProvider = DependencyService.Get<IAppVersionProvider>();

                var feedback = new ConferenceFeedback
                                   {
                                       Question1 = Question1,
                                       Question2 = Question2,
                                       Question3 = Question3,
                                       Question4 = Question4,
                                       Question5 = Question5,
                                       Question6 = Question6,
                                       Question7 = Question7,
                                       Question8 = Question8,
                                       Question9 = Question9,
                                       Question10 = Question10,
                                       DeviceOS = Device.RuntimePlatform,
                                       AppVersion = versionProvider.AppVersion,
                                   };

                await StoreManager.ConferenceFeedbackStore.InsertAsync(feedback);

                Settings.Current.LeaveConferenceFeedback();
            }
            catch (Exception ex)
            {
                Logger.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
