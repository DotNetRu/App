namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public class SpeakerCell: ViewCell
    {
        readonly INavigation navigation;

        readonly string sessionId;
        public SpeakerCell (string sessionId, INavigation navigation = null)
        {
            this.sessionId = sessionId;
            this.Height = 60;
            this.View = new SpeakerCellView ();
            this.StyleId = "disclosure";
            this.navigation = navigation;
        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (this.navigation == null)
                return;

            if (!(this.BindingContext is Speaker speaker))
            {
                return;
            }

            App.Logger.TrackPage(AppPage.Speaker.ToString(), speaker.FullName);

            if (Device.RuntimePlatform == Device.UWP)
            {
                await this.navigation.PushAsync(new SpeakerDetailsPageUWP(this.sessionId)
                {
                    Speaker = speaker
                });
            }
            else
            {
                await this.navigation.PushAsync(new SpeakerDetailsPage(this.sessionId)
                {
                    Speaker = speaker
                });
            }
        }
    }
    public partial class SpeakerCellView : ContentView
    {
        public SpeakerCellView()
        {
            this.InitializeComponent();
        }
    }
}

