using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
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

