using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	public partial class SyncMobileToWebPage : BasePage
	{
		public override AppPage PageType => AppPage.SyncMobileToWeb;

		SyncMobileToWebViewModel vm;
        public SyncMobileToWebPage()
        {
            InitializeComponent();
			BindingContext = vm = new SyncMobileToWebViewModel(Navigation);
        }
    }
}

