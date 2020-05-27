using System;
using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Models.Social;
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

            this.ListViewSubscriptions.ItemSelected += async (sender, e) =>
            {
                if (!(this.ListViewSubscriptions.SelectedItem is SubscriptionModel subscription))
                {
                    return;
                }

                await (this.BindingContext as SubscriptionsViewModel)?.ExecuteLaunchBrowserAsync(
                    subscription.Type == SocialMediaType.Vkontakte
                        ? subscription.Community?.VkUrl ?? new Uri($"https://vk.com/{subscription.Community.Name}")
                        : new Uri($"https://twitter.com/{subscription.Community.Name}"));

                this.ListViewSubscriptions.SelectedItem = null;
            };
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
