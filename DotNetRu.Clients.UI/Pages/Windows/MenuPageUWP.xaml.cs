
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Windows
{
    public partial class MenuPageUWP
    {
        public MenuPageUWP()
        {
            this.InitializeComponent();

            this.Title = "TODO";
        }

        public ListView MenuList => this.ListViewMenu;
    }
}
