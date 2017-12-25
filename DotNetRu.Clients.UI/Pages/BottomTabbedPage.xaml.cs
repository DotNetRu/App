namespace XamarinEvolve.Clients.UI.Pages
{
    using Xamarin.Forms.Xaml;

    using XamarinEvolve.Clients.Portable.ViewModel;

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