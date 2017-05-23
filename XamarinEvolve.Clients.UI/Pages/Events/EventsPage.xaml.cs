using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.Clients.UI
{
	public partial class EventsPage : BasePage
	{
		public override AppPage PageType => AppPage.Events;

        EventsViewModel vm;
        EventsViewModel ViewModel => vm ?? (vm = BindingContext as EventsViewModel);

		public EventsPage()
        {
            InitializeComponent();
            BindingContext = new EventsViewModel(Navigation);

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon ="toolbar_refresh.png",
                    Command = ViewModel.ForceRefreshCommand
                });
            }

            ListViewEvents.ItemTapped += (sender, e) => ListViewEvents.SelectedItem = null;
            ListViewEvents.ItemSelected += async (sender, e) => 
                {
                    var ev = ListViewEvents.SelectedItem as FeaturedEvent;
                    if(ev == null)
                        return;
                    
                    var eventDetails = new EventDetailsPage();

                    eventDetails.Event = ev;
                    await NavigationService.PushAsync(Navigation, eventDetails);

                    ListViewEvents.SelectedItem = null;
                };
        }
            
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Events.Count == 0)
                ViewModel.LoadEventsCommand.Execute(false);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}

