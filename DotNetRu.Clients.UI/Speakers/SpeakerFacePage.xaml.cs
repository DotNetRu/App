namespace DotNetRu.Clients.UI.Pages.Speakers
{
    using System.IO;

    using DotNetRu.Clients.Portable.Model;

    using Xamarin.Forms;

    public partial class SpeakerFacePage
    {
        public SpeakerFacePage(byte[] image)
        {
            this.InitializeComponent();
            this.faceImage.Source = ImageSource.FromStream(() => new MemoryStream(image));
        }

        public override AppPage PageType => AppPage.SpeakerFace;
    }
}
