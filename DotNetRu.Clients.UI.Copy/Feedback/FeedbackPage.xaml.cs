using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.DataStore.Audit.Models;
using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Pages.Sessions
{
    public partial class FeedbackPage : BasePage
	{
		public override AppPage PageType => AppPage.Feedback;

	    readonly FeedbackViewModel vm;

        public FeedbackPage(TalkModel talkModel)
        {
            this.InitializeComponent();

            this.ItemId = talkModel.Title;

            this.BindingContext = this.vm = new FeedbackViewModel(this.Navigation, talkModel);
            if (Device.RuntimePlatform != Device.iOS) this.ToolbarDone.Icon = "toolbar_close.png";

            this.ToolbarDone.Command = new Command(async () => 
                {
                    if(this.vm.IsBusy)
                        return;
                    
                    await this.Navigation.PopModalAsync();
                });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.RatingControl.RemoveBehaviors();
        }
    }
}

