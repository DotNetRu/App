using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            this.Padding = 0;
            if (Device.RuntimePlatform == Device.iOS)
            {
                this.HasShadow = false;
                this.OutlineColor = Color.Transparent;
                this.BackgroundColor = Color.Transparent;
            }
        }
    }
}

