using DotNetRu.Clients.Portable.Model;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Speakers
{
    public partial class SpeakerFacePage
    {
        public SpeakerFacePage(ImageSource image)
        {
            InitializeComponent();
            faceImage.Source = image;
        }

        public override AppPage PageType => AppPage.SpeakerFace;
    }
}
