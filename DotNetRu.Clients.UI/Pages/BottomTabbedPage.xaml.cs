using DotNetRu.Clients.Portable.ViewModel;
using Xamarin.Forms.Xaml;

namespace DotNetRu.Clients.UI.Pages
{
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