using DotNetRu.Clients.UI.Localization;

namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using DotNetRu.DataStore.Audit.Helpers;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Models.Social;
    using DotNetRu.Utils;
    using DotNetRu.Utils.Helpers;
    using FormsToolkit;
    using MvvmHelpers;
    using Xamarin.Forms;

    public class SubscriptionsViewModel : ViewModelBase
    {
        private ICommand loadSubscriptionsCommand;

        public SubscriptionsViewModel(INavigation navigation)
            : base(navigation)
        {
        }

        public ObservableRangeCollection<Grouping<string, SubscriptionModel>> SubscriptionItems { get; } = new ObservableRangeCollection<Grouping<string, SubscriptionModel>>();

        public ICommand LoadSubscriptionsCommand => this.loadSubscriptionsCommand ??= new Command(this.ExecuteLoadSubscriptions);

        private void ExecuteLoadSubscriptions()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;

                UpdateSubscriptions();
            }
            catch (Exception ex)
            {
                DotNetRuLogger.Report(ex);
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void UpdateSubscriptions()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var communitySubscriptions = Helpers.Settings.CommunitySubscriptions.GroupBy(x => x.Community?.City, x => x);
                this.SubscriptionItems.ReplaceRange(communitySubscriptions.Select(x => new Grouping<string, SubscriptionModel>(x.Key, x)));
            });
        }

        public ICommand SelectedItemCommand => new Command<SubscriptionModel>(async item => await HandleSelection(item));

        private async Task HandleSelection(SubscriptionModel item)
        {
            switch (item.Type)
            {
                case SocialMediaType.Vkontakte:
                    await ExecuteLaunchBrowserAsync(item.Community.VkUrl);
                    break;
                case SocialMediaType.Twitter:
                    await ExecuteLaunchBrowserAsync(new Uri($"https://twitter.com/{item.Community.Name}"));
                    break;
            }
        }

        public ICommand SaveSubscriptionsCommand => new Command(async () => await SaveSubscriptionsAsync());

        internal async Task SaveSubscriptionsAsync()
        {
            var temp = new List<SubscriptionModel>();
            foreach (var subscriptionItem in SubscriptionItems)
            {
                temp.AddRange(subscriptionItem.Items);
            }
            Helpers.Settings.CommunitySubscriptions = temp;
            await Application.Current.MainPage.DisplayAlert(string.Empty, Resources[nameof(AppResources.SubscriptionsSavedMessage)], "OK");
        }

        public ICommand ResetSubscriptionsCommand => new Command(async () => await ResetSubscriptionsAsync());

        internal async Task ResetSubscriptionsAsync()
        {
            Helpers.Settings.CommunitySubscriptions = SubscriptionsHelper.DefaultCommunitySubscriptions;
            UpdateSubscriptions();
            await Application.Current.MainPage.DisplayAlert(string.Empty, Resources[nameof(AppResources.SubscriptionsResetedMessage)], "OK");
        }
    }
}
