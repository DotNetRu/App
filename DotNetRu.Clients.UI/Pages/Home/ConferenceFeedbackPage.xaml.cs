using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            InitializeComponent();

			BindingContext = vm = new ConferenceFeedbackViewModel(Navigation);

			if (Device.OS != TargetPlatform.iOS)
				ToolbarDone.Icon = "toolbar_close.png";

			ToolbarDone.Command = new Command(async () =>
				{
					if (vm.IsBusy)
						return;

					await Navigation.PopModalAsync();
				});
        }

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			Question1.RemoveBehaviors();
			Question2.RemoveBehaviors();
			Question3.RemoveBehaviors();
			Question4.RemoveBehaviors();
			Question5.RemoveBehaviors();
			Question6.RemoveBehaviors();
			Question7.RemoveBehaviors();
		}
    }
}
