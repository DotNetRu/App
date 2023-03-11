namespace DotNetRu.Clients.UI.Controls
{
    using Microsoft.Maui;

    public class CardView : Frame
    {
        public CardView()
        {
            this.Padding = 0;
            if (Device.RuntimePlatform == Device.iOS)
            {
                this.HasShadow = false;
                this.BorderColor = Color.Transparent;
                this.BackgroundColor = Color.Transparent;
            }
        }
    }
}

