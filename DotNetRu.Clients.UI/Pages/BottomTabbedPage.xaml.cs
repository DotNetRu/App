using DotNetRu.Clients.Portable.ViewModel;

namespace XamarinEvolve.Clients.UI.Pages
{
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomTabbedPage
    {
        public BottomTabbedPage()
        {
            this.InitializeComponent();
            this.BindingContext = new BottomBarViewModel();
        }
    }
}