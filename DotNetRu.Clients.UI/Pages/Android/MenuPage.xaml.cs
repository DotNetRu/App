namespace XamarinEvolve.Clients.UI
{
    using System.Threading.Tasks;

    using Xamarin.Forms;

    public partial class MenuPage : ContentPage
    {
        readonly RootPageAndroid root;

        public MenuPage(RootPageAndroid root)
        {
            this.root = root;
            this.InitializeComponent();

            this.Title = "TODO";

            this.NavView.NavigationItemSelected += async (sender, e) =>
                {
                    this.root.IsPresented = false;

                    await Task.Delay(225);
                    await this.root.NavigateAsync(e.Index);
                };
        }
    }
}
