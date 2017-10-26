namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils.Helpers;

    public class FeedbackViewModel : ViewModelBase
    {
        TalkModel talkModel;

        public TalkModel TalkModel
        {
            get => this.talkModel;
            set => this.SetProperty(ref this.talkModel, value);
        }


        public FeedbackViewModel(INavigation navigation, TalkModel talkModel)
            : base(navigation)
        {
            this.TalkModel = talkModel;
        }

        ICommand submitRatingCommand;

        public ICommand SubmitRatingCommand => this.submitRatingCommand
                                               ?? (this.submitRatingCommand = new Command<int>(
                                                       async (rating) =>
                                                           await this.ExecuteSubmitRatingCommandAsync(rating)));

        async Task ExecuteSubmitRatingCommandAsync(int rating)
        {
            if (this.IsBusy) return;

            this.IsBusy = true;
            try
            {
                if (rating == 0)
                {
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(
                        MessageKeys.Message,
                        new MessagingServiceAlert
                            {
                                Title = "No Rating Selected",
                                Message =
                                    "Please select a rating to leave feedback for this session.",
                                Cancel = "OK"
                            });
                    return;
                }

                this.Logger.Track(EvolveLoggerKeys.LeaveFeedback, "Title", rating.ToString());

                MessagingService.Current.SendMessage<MessagingServiceAlert>(
                    MessageKeys.Message,
                    new MessagingServiceAlert
                        {
                            Title = "Feedback Received",
                            Message = $"Thanks for the feedback!",
                            Cancel = "OK",
                            OnCompleted = async () =>
                                {
                                    await this.Navigation.PopModalAsync();
                                    if (Device.RuntimePlatform == Device.Android)
                                        MessagingService.Current.SendMessage("eval_finished");
                                }
                        });

                this.TalkModel.FeedbackLeft = true;
                await this.StoreManager.FeedbackStore.InsertAsync(
                    new Feedback { SessionId = this.talkModel.Id, SessionRating = rating });
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

