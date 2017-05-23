
using Xamarin.Forms;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	public partial class MenuPageUWP : ContentPage
	{
		public MenuPageUWP()
		{
			InitializeComponent();

			Title = EventInfo.EventName;
		}

		public ListView MenuList => ListViewMenu;
	}
}
