using System.Threading.Tasks;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI
{
    public class CustomTab : Tab
    {
		protected override Task<Page> OnPopAsync(bool animated)
		{
			// temporary workaround while https://github.com/xamarin/Xamarin.Forms/issues/8581 not fixed
			return base.OnPopAsync(animated: false);
		}
	}
}
