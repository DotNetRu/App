namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;

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

        private IEnumerable<FriendModel> LoadFriends()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.friends.xml");
            IEnumerable<FriendEntity> friendEntities;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = "Friends", IsNullable = false };
                var serializer = new XmlSerializer(typeof(List<FriendEntity>), xRoot);
                friendEntities = (List<FriendEntity>)serializer.Deserialize(reader);
            }

            return friendEntities.Select(x => x.ToModel());
        }

        private void ExecuteLoadFriends()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;
                this.Friends.ReplaceRange(this.LoadFriends()); // await StoreManager.SponsorStore.GetItemsAsync(force);
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