namespace DotNetRu.Clients.UI.Pages.Speakers
{
    using DotNetRu.Clients.Portable.Model;

    public partial class SpeakerFacePage
    {
        public string AvatarURL { get; set; }

        public SpeakerFacePage(string speakerID, string avatarURL)
        {
            this.InitializeComponent();
            this.AvatarURL = avatarURL;

            this.ItemId = speakerID;

            this.BindingContext = this;
        }

        public override AppPage PageType => AppPage.SpeakerFace;
    }
}
