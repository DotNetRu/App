namespace DotNetRu.Clients.UI.Cells
{
    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Utils.Helpers;

    using Xamarin.Forms;

    public partial class CommunityViewCell
    {
        public static readonly BindableProperty CommunityProperty =
            BindableProperty.Create(nameof(Community), typeof(CommunityModel), typeof(CommunityViewCell));

        public CommunityViewCell()
        {
            this.InitializeComponent();

            MessagingCenter.Subscribe<LocalizedResources>(
                this,
                MessageKeys.LanguageChanged,
                sender => this.OnPropertyChanged(nameof(this.Community)));
        }

        public CommunityModel Community
        {
            get => (CommunityModel)this.GetValue(CommunityProperty);
            set => this.SetValue(CommunityProperty, value);
        }
    }
}