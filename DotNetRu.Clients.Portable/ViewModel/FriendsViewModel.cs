namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Windows.Input;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.Services;

    using FormsToolkit;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.Utils.Helpers;

    public class FriendsViewModel : ViewModelBase
    {
        private ICommand loadFriendsCommand;
        private FriendModel selectedFriendModel;

        public FriendsViewModel(INavigation navigation)
            : base(navigation)
        {
        }

        public ObservableRangeCollection<FriendModel> Friends { get; } = new ObservableRangeCollection<FriendModel>();
       

        public FriendModel SelectedFriendModel
        {
            get => this.selectedFriendModel;
            set
            {
                this.selectedFriendModel = value;
                this.OnPropertyChanged();
                if (this.selectedFriendModel == null)
                {
                    return;
                }

                MessagingService.Current.SendMessage(MessageKeys.NavigateToSponsor, this.selectedFriendModel);

                this.SelectedFriendModel = null;
            }
        }

        public ICommand LoadFriendsCommand => this.loadFriendsCommand
                                              ?? (this.loadFriendsCommand =
                                                      new Command(this.ExecuteLoadFriends));

        private void ExecuteLoadFriends()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;
                this.Friends.ReplaceRange(FriendService.Friends);
            }
            catch (Exception ex)
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadFriends");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}