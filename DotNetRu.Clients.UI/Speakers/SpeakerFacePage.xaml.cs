namespace DotNetRu.Clients.UI.Pages.Speakers
{
    using DotNetRu.Clients.Portable.Model;

    public partial class SpeakerFacePage
    {
        public string AvatarURL { get; set; }

        public SpeakerFacePage(string avatarURL)
        {
            this.InitializeComponent();
            this.AvatarURL = avatarURL;

            this.BindingContext = this;
        }

        public override AppPage PageType => AppPage.SpeakerFace;
    }
}
