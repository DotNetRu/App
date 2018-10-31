namespace DotNetRu.Clients.UI.Cells
{
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Sessions;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public class SessionCell : ViewCell
    {
        private readonly INavigation navigation;

        public SessionCell(INavigation navigation = null)
        {
            this.Height = 120;
            this.View = new SessionCellView();
            this.navigation = navigation;
        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (this.navigation == null)
            {
                return;
            }

            if (!(this.BindingContext is TalkModel session))
            {
                return;
            }

            App.Logger.TrackPage(AppPage.Talk.ToString(), session.Title);
            await NavigationService.PushAsync(this.navigation, new TalkPage(session));
        }
    }

    public partial class SessionCellView
    {
        public SessionCellView()
        {
            this.InitializeComponent();
        }
    }
}