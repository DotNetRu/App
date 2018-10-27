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

        EventsViewModel ViewModel => this.eventsViewModel ?? (this.eventsViewModel = BindingContext as EventsViewModel);

        public EventsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new EventsViewModel(Navigation);

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

            this.ListViewEvents.ItemTapped += (sender, e) => ListViewEvents.SelectedItem = null;
            this.ListViewEvents.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewEvents.SelectedItem is FeaturedEvent ev))
                    {
                        return;
                    }

                    var eventSessions = new SessionsPage(ev);
                    await NavigationService.PushAsync(Navigation, eventSessions);
                    ListViewEvents.SelectedItem = null;
                };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Events.Count == 0) ViewModel.LoadEventsCommand.Execute(false);
        }
    }
}

