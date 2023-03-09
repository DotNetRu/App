namespace DotNetRu.Clients.Portable.ViewModel
{
    using System;
    using System.Windows.Input;
    using DotNetRu.AppUtils;
    using DotNetRu.AppUtils.Logging;
    using DotNetRu.DataStore.Audit.Models;

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
                                             new Command(() => this.ExecuteLoadNotifications(true)));

        ICommand loadNotificationsCommand;

        public ICommand LoadNotificationsCommand =>
            this.loadNotificationsCommand ?? (this.loadNotificationsCommand =
                                                  new Command<bool>(
                                                      (f) => this.ExecuteLoadNotifications()));

        private bool ExecuteLoadNotifications(bool force = false)
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
                DotNetRuLogger.Report(ex);
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

