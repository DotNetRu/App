using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    public partial class FeedbackPage : BasePage
	{
		public override AppPage PageType => AppPage.Feedback;
        FeedbackViewModel vm;

        public FeedbackPage(Session session)
        {
            InitializeComponent();

			ItemId = session.Title;

            BindingContext = vm = new FeedbackViewModel(Navigation, session);
            if (Device.OS != TargetPlatform.iOS)
                ToolbarDone.Icon = "toolbar_close.png";

            ToolbarDone.Command = new Command(async () => 
                {
                    if(vm.IsBusy)
                        return;
                    
                    await Navigation.PopModalAsync();
                });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            RatingControl.RemoveBehaviors();
        }
    }
}

