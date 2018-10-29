namespace DotNetRu.Clients.UI.Pages
{
    using DotNetRu.Clients.Portable.ViewModel;

    public partial class BottomTabbedPage
    {
        public BottomTabbedPage()
        {
            this.InitializeComponent();
            this.BindingContext = new BottomBarViewModel();
        }
    }
}