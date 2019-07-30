namespace DotNetRu.Clients.UI.Pages.Speakers
{
    using System;
    using DotNetRu.Clients.Portable.Model;

    public partial class SpeakerFacePage
    {
        public Uri AvatarURL { get; set; }

        public SpeakerFacePage(string speakerID, Uri avatarURL)
        {
            this.InitializeComponent();
            this.AvatarURL = avatarURL;

            this.ItemId = speakerID;

            this.BindingContext = this;
        }

        public override AppPage PageType => AppPage.SpeakerFace;
    }
}
