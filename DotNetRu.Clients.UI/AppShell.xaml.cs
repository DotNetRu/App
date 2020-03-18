using Xamarin.Forms;

namespace DotNetRu.Clients.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();

            var primaryColor = (Color)Application.Current.Resources["Primary"];

            var backgroundColor = Device.RuntimePlatform == Device.iOS ? Color.White : primaryColor;

            // Can't set BackgrounColor as StaticResource, see https://github.com/xamarin/Xamarin.Forms/issues/7055
            SetBackgroundColor(this, backgroundColor);
        }
    }
}
