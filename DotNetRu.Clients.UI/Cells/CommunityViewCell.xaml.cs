namespace DotNetRu.Clients.UI.Cells
{
    using DotNetRu.Clients.Portable.Model;

    using Xamarin.Forms;

    public partial class CommunityViewCell
    {
        public static readonly BindableProperty CommunityProperty =
            BindableProperty.Create(nameof(Community), typeof(CommunityModel), typeof(CommunityViewCell));

        public CommunityViewCell()
        {
            this.InitializeComponent();
        }

        public CommunityModel Community
        {
            get => (CommunityModel)this.GetValue(CommunityProperty);
            set => this.SetValue(CommunityProperty, value);
        }
    }
}