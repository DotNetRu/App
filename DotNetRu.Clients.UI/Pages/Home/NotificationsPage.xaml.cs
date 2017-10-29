using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	public partial class NotificationsPage : BasePage
	{
		public override AppPage PageType => AppPage.Notification;

	    readonly NotificationsViewModel vm;
        public NotificationsPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = new NotificationsViewModel();
            this.ListViewNotifications.ItemTapped += (sender, e) => this.ListViewNotifications.SelectedItem = null;
        }

		protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.vm.Notifications.Count == 0) this.vm.LoadNotificationsCommand.Execute(false);
        }
    }
}

