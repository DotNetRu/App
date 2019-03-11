namespace DotNetRu.Clients.UI.Controls
{
    using Xamarin.Forms;

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

