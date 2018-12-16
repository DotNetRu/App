namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    public class NotificationsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Notification> Notifications { get; } =
            new ObservableRangeCollection<Notification>();

        public ObservableRangeCollection<Grouping<string, Notification>> NotificationsGrouped { get; } =
            new ObservableRangeCollection<Grouping<string, Notification>>();

        ICommand forceRefreshCommand;

        public ICommand ForceRefreshCommand =>
            this.forceRefreshCommand ?? (this.forceRefreshCommand =
                                             new Command(async () => await this.ExecuteForceRefreshCommandAsync()));

        async Task ExecuteForceRefreshCommandAsync()
        {
            await this.ExecuteLoadNotificationsAsync(true);
        }

        ICommand loadNotificationsCommand;

        public ICommand LoadNotificationsCommand =>
            this.loadNotificationsCommand ?? (this.loadNotificationsCommand =
                                                  new Command<bool>(
                                                      async (f) => await this.ExecuteLoadNotificationsAsync()));

        async Task<bool> ExecuteLoadNotificationsAsync(bool force = false)
        {
            if (this.IsBusy)
            {
                return false;
            }
            try
            {
                this.IsBusy = true;
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadNotificationsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return true;
        }
    }
}

