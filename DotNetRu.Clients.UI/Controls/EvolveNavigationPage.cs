
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Controls
{
    public class EvolveNavigationPage : NavigationPage
    {
        public EvolveNavigationPage(Page root) : base(root)
        {
            this.Init();
            this.Title = root.Title;
            this.Icon = root.Icon;
        }

        public EvolveNavigationPage()
        {
            this.Init();
        }

        void Init()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                this.BarBackgroundColor = Color.FromHex("FAFAFA");
            }
            else
            {
                this.BarBackgroundColor = (Color)Application.Current.Resources["Primary"];
                this.BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            }
        }
    }
}

