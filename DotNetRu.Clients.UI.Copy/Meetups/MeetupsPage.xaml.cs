namespace DotNetRu.Clients.UI.Pages.Meetups
{
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Sessions;
    using DotNetRu.DataStore.Audit.Models;

    public partial class MeetupsPage
    {
        private MeetupsViewModel meetupsViewModel;

        public MeetupsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new MeetupsViewModel(this.Navigation);

            this.ListViewEvents.ItemTapped += (sender, e) => this.ListViewEvents.SelectedItem = null;
            this.ListViewEvents.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewEvents.SelectedItem is MeetupModel ev))
                    {
                        return;
                    }

                    var eventSessions = new MeetupDetailsPage(ev);
                    await NavigationService.PushAsync(this.Navigation, eventSessions);
                    this.ListViewEvents.SelectedItem = null;
                };
        }

        public MeetupsViewModel MeetupsViewModel =>
            this.meetupsViewModel ?? (this.meetupsViewModel = this.BindingContext as MeetupsViewModel);

        public override AppPage PageType => AppPage.Meetups;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // TODO flag/message indicating that update needed
            this.MeetupsViewModel.LoadMeetupsCommand.Execute(false);
        }
    }
}
