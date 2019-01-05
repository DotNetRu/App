namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Windows.Input;
    using DotNetRu.Utils;
    using DotNetRu.Utils.Helpers;
    using DotNetRu.Utils.Interfaces;

    using FormsToolkit;

    using Xamarin.Forms;

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
                                                         this.ExecuteSubmitFeedbackCommandAsync));

        private void ExecuteSubmitFeedbackCommandAsync()
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

                this.Logger.Track(DotNetRuLoggerKeys.ConferenceFeedback);

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

                Settings.Current.LeaveConferenceFeedback();
            }
            catch (Exception ex)
            {
                DotNetRuLogger.Report(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
