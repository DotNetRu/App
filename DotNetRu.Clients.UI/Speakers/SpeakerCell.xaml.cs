using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.UI.Pages.Speakers;
using DotNetRu.Clients.UI.Speakers;
using DotNetRu.DataStore.Audit.Models;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Cells
{
    public class SpeakerCell : ViewCell
    {
        readonly INavigation navigation;

        readonly string sessionId;

        public SpeakerCell(string sessionId, INavigation navigation = null)
        {
            this.sessionId = sessionId;
            this.Height = 60;
            this.View = new SpeakerCellView();
            this.StyleId = "disclosure";
            this.navigation = navigation;
        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (this.navigation == null)
            {
                return;
            }

            if (!(this.BindingContext is SpeakerModel speaker))
            {
                return;
            }

            App.Logger.TrackPage(AppPage.Speaker.ToString(), speaker.FullName);

            await this.navigation.PushAsync(new SpeakerDetailsPage { SpeakerModel = speaker });
        }
    }

    public partial class SpeakerCellView
    {
        public SpeakerCellView()
        {
            this.InitializeComponent();
        }
    }
}

