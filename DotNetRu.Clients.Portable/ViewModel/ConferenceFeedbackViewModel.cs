namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Utils.Helpers;

    public class ConferenceFeedbackViewModel : ViewModelBase
    {
        public ConferenceFeedbackViewModel(INavigation navigation)
            : base(navigation)
        {
            this.Title = "Conference Feedback";
        }

        private int _question;

        public int Question
        {
            get => this._question;
            set => this.SetProperty(ref this._question, value);
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
                if (this.Question == 0)
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
                                       Question1 = this.Question,
                                       DeviceOS = Device.RuntimePlatform,
                                       AppVersion = versionProvider.AppVersion,
                                   };

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
