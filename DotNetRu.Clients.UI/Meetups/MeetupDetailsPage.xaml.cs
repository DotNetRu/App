using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.UI.Meetups;
using DotNetRu.DataStore.Audit.Models;

namespace DotNetRu.Clients.UI.Pages.Sessions
{
    public partial class MeetupDetailsPage
    {
        public MeetupDetailsPage(MeetupModel meetup)
        {
            this.InitializeComponent();

            this.BindingContext = new MeetupDetailsViewModel(this.Navigation, meetup);

            this.ItemId = meetup.Id;
        }

        public override AppPage PageType => AppPage.Meetup;
    }
}
