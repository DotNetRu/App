using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Controls
{
    public class NonScrollableListView : ListView
    {
        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                this.BackgroundColor = Color.White;
            }
        }
    }
}

