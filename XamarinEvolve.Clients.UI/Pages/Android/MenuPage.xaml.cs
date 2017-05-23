using Xamarin.Forms;
using System.Threading.Tasks;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	public partial class MenuPage : ContentPage
	{
		RootPageAndroid root;
		public MenuPage(RootPageAndroid root)
		{
			this.root = root;
			InitializeComponent();

			this.Title = EventInfo.EventName;

			NavView.NavigationItemSelected += async (sender, e) =>
			{
				this.root.IsPresented = false;

				await Task.Delay(225);
				await this.root.NavigateAsync(e.Index);
			};
		}
	}
}
