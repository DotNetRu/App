using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	public partial class ConferenceFeedbackPage : BasePage
    {
		public override AppPage PageType => AppPage.ConferenceFeedback;
		ConferenceFeedbackViewModel vm;

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
