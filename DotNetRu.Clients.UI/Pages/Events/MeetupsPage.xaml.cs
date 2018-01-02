using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Sessions;
using DotNetRu.DataStore.Audit.Models;

namespace DotNetRu.Clients.UI.Pages.Events
{
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
            if (!this.MeetupsViewModel.MeetupsByMonth.Any())
            {
                this.MeetupsViewModel.LoadMeetupsCommand.Execute(false);
            }
        }
    }
}
