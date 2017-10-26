namespace XamarinEvolve.Clients.UI
{
    using Xamarin.Forms;

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

