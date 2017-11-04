namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Clients.Portable.ViewModel;

    public partial class MeetupsPage
    {
        public override AppPage PageType => AppPage.Events;

        MeetupsViewModel meetupsViewModel;

        MeetupsViewModel ViewModel => this.meetupsViewModel ?? (this.meetupsViewModel = this.BindingContext as MeetupsViewModel);

        public MeetupsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new MeetupsViewModel(this.Navigation);

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
                    if (!(this.ListViewEvents.SelectedItem is MeetupModel ev))
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

