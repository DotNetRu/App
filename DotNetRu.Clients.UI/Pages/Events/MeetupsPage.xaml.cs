namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.Portable.ViewModel;

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

                    var eventSessions = new MeetupPage(ev);
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
            if (this.MeetupsViewModel.Events.Count == 0)
            {
                this.MeetupsViewModel.LoadEventsCommand.Execute(false);
            }
        }
    }
}

