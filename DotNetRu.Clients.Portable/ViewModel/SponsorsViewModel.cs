using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

using FormsToolkit;

using MvvmHelpers;

using Xamarin.Forms;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Models;

    using XamarinEvolve.Utils.Helpers;

	public class SponsorsViewModel : ViewModelBase
    {
        public SponsorsViewModel(INavigation navigation) : base(navigation)
        {

        }

		public ObservableRangeCollection<Grouping<string, Sponsor>> SponsorsGrouped { get; } = new ObservableRangeCollection<Grouping<string, Sponsor>>();

        #region Properties
        Sponsor selectedSponsor;
        public Sponsor SelectedSponsor
        {
            get => this.selectedSponsor;
            set
            {
                this.selectedSponsor = value;
                this.OnPropertyChanged();
                if (this.selectedSponsor == null)
                    return;
                 
                MessagingService.Current.SendMessage(MessageKeys.NavigateToSponsor, this.selectedSponsor);

                this.SelectedSponsor = null;
            }
        }
		     
        #endregion

        #region Sorting


        void SortSponsors(IEnumerable<Sponsor> sponsors)
        {
            var groups = from sponsor in sponsors
				orderby sponsor.SponsorLevel?.Rank ?? 9999
				group sponsor by sponsor.SponsorLevel?.Name ?? "Sponsor"
                into sponsorGroup
                select new Grouping<string, Sponsor>(sponsorGroup.Key, sponsorGroup.OrderBy(s => s.Rank));

            this.SponsorsGrouped.ReplaceRange(groups);
        }

        #endregion


        #region Commands

        ICommand  forceRefreshCommand;
        public ICommand ForceRefreshCommand => this.forceRefreshCommand ?? (this.forceRefreshCommand = new Command(async () => await this.ExecuteForceRefreshCommandAsync())); 

        async Task ExecuteForceRefreshCommandAsync()
        {
            await this.ExecuteLoadSponsorsAsync(true);
        }

        ICommand loadSponsorsCommand;
        public ICommand LoadSponsorsCommand => this.loadSponsorsCommand ?? (this.loadSponsorsCommand = new Command(async (f) => await this.ExecuteLoadSponsorsAsync()));

        List<Sponsor> FriendToSponsorConverter(IEnumerable<Friend> friends)
        {
            return friends.Select(friend => new Sponsor
            {
                Description = friend.Description,
                Name = friend.Name,
                Id = friend.Id,
                ImageUrl = "https://d30y9cdsu7xlg0.cloudfront.net/png/6808-200.png",
                WebsiteUrl = friend.Url
            }).ToList();
        }


        List<Sponsor> GetSponsors()
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Storage.friends.xml");
            IEnumerable<Friend> friends;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute
                {
                    ElementName = "Friends",
                    IsNullable = false
                };
                var serializer = new XmlSerializer(typeof(List<Friend>), xRoot);
                friends = (List<Friend>)serializer.Deserialize(reader); 
            }

            return this.FriendToSponsorConverter(friends);
        }

        async Task<bool> ExecuteLoadSponsorsAsync(bool force = false)
        {
            if(this.IsBusy)
                return false;

            try 
            {
                this.IsBusy = true;

                #if DEBUG
                await Task.Delay(1000);
                #endif
                var sponsors = this.GetSponsors();// await StoreManager.SponsorStore.GetItemsAsync(force);

                this.SortSponsors(sponsors);

            } 
            catch (Exception ex) 
            {
                this.Logger.Report(ex, "Method", "ExecuteLoadSponsorsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                this.IsBusy = false;
            }

            return true;
        }
           

        #endregion
    }
}

