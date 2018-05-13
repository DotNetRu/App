namespace DotNetRu.Clients.UI.Cells
{
    using System.Windows.Input;

    using DotNetRu.Clients.Portable.Model;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommunityViewCell : ViewCell
    {
        public static readonly BindableProperty LaunchBrowserCommandProperty =
            BindableProperty.Create(nameof(LaunchBrowserCommand), typeof(string), typeof(CommunityViewCell));

        public static readonly BindableProperty CommunityProperty =
            BindableProperty.Create(nameof(Community), typeof(string), typeof(CommunityViewCell));

        public CommunityViewCell()
        {
            InitializeComponent();
        }

        public CommunityModel Community
        {
            get => (CommunityModel)this.GetValue(CommunityProperty);
            set => this.SetValue(CommunityProperty, value);
        }

        public ICommand LaunchBrowserCommand
        {
            get => (ICommand)this.GetValue(LaunchBrowserCommandProperty);
            set => this.SetValue(LaunchBrowserCommandProperty, value);
        }
    }
}