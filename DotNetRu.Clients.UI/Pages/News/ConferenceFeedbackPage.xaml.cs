using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Home
{
	public partial class ConferenceFeedbackPage : BasePage
    {
		public override AppPage PageType => AppPage.ConferenceFeedback;

        readonly ConferenceFeedbackViewModel vm;

        public ConferenceFeedbackPage()
        {
            this.InitializeComponent();

            this.BindingContext = this.vm = new ConferenceFeedbackViewModel(this.Navigation);

			if (Device.RuntimePlatform != Device.iOS) this.ToolbarDone.Icon = "toolbar_close.png";

            this.ToolbarDone.Command = new Command(async () =>
				{
					if (this.vm.IsBusy)
						return;

					await this.Navigation.PopModalAsync();
				});
        }
    }
}
