
using Xamarin.Forms;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	using XamarinEvolve.Utils.Helpers;

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
