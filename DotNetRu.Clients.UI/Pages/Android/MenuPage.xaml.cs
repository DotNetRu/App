namespace XamarinEvolve.Clients.UI
{
    using System.Threading.Tasks;

    using Xamarin.Forms;

    public partial class MenuPage : ContentPage
    {
        RootPageAndroid root;

        public MenuPage(RootPageAndroid root)
        {
            this.root = root;
            InitializeComponent();

            this.Title = "TODO";

            NavView.NavigationItemSelected += async (sender, e) =>
                {
                    this.root.IsPresented = false;

                    await Task.Delay(225);
                    await this.root.NavigateAsync(e.Index);
                };
        }
    }
}
