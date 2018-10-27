namespace XamarinEvolve.Clients.UI
{
    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.Portable.ViewModel;
    using XamarinEvolve.DataObjects;

    public partial class EventsPage
    {
        public override AppPage PageType => AppPage.Events;

        EventsViewModel eventsViewModel;

        EventsViewModel ViewModel => this.eventsViewModel ?? (this.eventsViewModel = this.BindingContext as EventsViewModel);

        public EventsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new EventsViewModel(this.Navigation);

            if (Device.RuntimePlatform == Device.UWP)
            {
                this.ToolbarItems.Add(
                    new ToolbarItem
                        {
                            Text = "Refresh",
                            Icon = "toolbar_refresh.png",
                            Command = this.ViewModel.ForceRefreshCommand
                        });
            }

            this.ListViewEvents.ItemTapped += (sender, e) => this.ListViewEvents.SelectedItem = null;
            this.ListViewEvents.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewEvents.SelectedItem is FeaturedEvent ev))
                    {
                        return;
                    }

                    var eventSessions = new MeetupPage(ev);
                    await NavigationService.PushAsync(this.Navigation, eventSessions);
                    this.ListViewEvents.SelectedItem = null;
                };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.ViewModel.Events.Count == 0) this.ViewModel.LoadEventsCommand.Execute(false);
        }
    }
}

