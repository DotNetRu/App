using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Subscriptions
{
    public partial class SubscriptionsPage
    {
        private SubscriptionsViewModel subscriptionsViewModel;

        public SubscriptionsPage()
        {
            this.InitializeComponent();
            this.BindingContext = new SubscriptionsViewModel(this.Navigation);
        }

        public SubscriptionsViewModel SubscriptionsViewModel =>
            this.subscriptionsViewModel ??= this.BindingContext as SubscriptionsViewModel;

        public override AppPage PageType => AppPage.Subscriptions;

        public void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!this.SubscriptionsViewModel.SubscriptionItems.Any())
            {
                this.SubscriptionsViewModel.LoadSubscriptionsCommand.Execute(true);
            }

            this.ListViewSubscriptions.ItemTapped += this.ListViewTapped;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewSubscriptions.ItemTapped -= this.ListViewTapped;
        }
    }
}
