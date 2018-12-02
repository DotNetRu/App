namespace DotNetRu.Clients.UI.Pages
{
    using DotNetRu.Clients.Portable.ViewModel;

    using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

    public partial class BottomTabbedPage
    {
        public BottomTabbedPage()
        {
            this.InitializeComponent();

            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            this.BindingContext = new BottomBarViewModel();
        }
    }
}