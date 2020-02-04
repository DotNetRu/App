using System;
using System.Collections.Generic;
using System.Linq;

using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Controls;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Speakers;
using DotNetRu.DataStore.Audit.Models;

using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Sessions
{
    public partial class TalkPage
    {
        private TalkViewModel talkViewModel;

        public TalkPage(TalkModel talkModel)
        {
            this.InitializeComponent();

            this.ItemId = talkModel?.Title;

            this.ListViewSpeakers.ItemSelected += OnSpeakerSelected;
            this.ListViewSeeAlsoTalks.ItemSelected += OnSeeAlsoTalkSelected;

            this.BindingContext = new TalkViewModel(this.Navigation, talkModel);
        }

        private async void OnSpeakerSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(this.ListViewSpeakers.SelectedItem is SpeakerModel speaker))
            {
                return;
            }

            var speakerDetails =
               new SpeakerDetailsPage() { SpeakerModel = speaker };

             await NavigationService.PushAsync(this.Navigation, speakerDetails);
            this.ListViewSpeakers.SelectedItem = null;
        }

        private async void OnSeeAlsoTalkSelected(object sender, SelectedItemChangedEventArgs e)
        {            
            if (!(this.ListViewSeeAlsoTalks.SelectedItem is TalkModel talk))
            {
                return;
            }

            var sessionDetails = new TalkPage(talk);

            await NavigationService.PushAsync(this.Navigation, sessionDetails);
            this.ListViewSeeAlsoTalks.SelectedItem = null;
        }

        public override AppPage PageType => AppPage.Talk;

        public TalkViewModel ViewModel => this.talkViewModel ?? (this.talkViewModel = this.BindingContext as TalkViewModel);

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
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

            ListViewSpeakers.ItemTapped += ListViewTapped;
            ListViewSeeAlsoTalks.ItemTapped += ListViewTapped;            

            var talkModel = ViewModel?.TalkModel;
            AdjustListView(ListViewSpeakers, talkModel?.Speakers);
            AdjustListView(ListViewSeeAlsoTalks, talkModel?.SeeAlsoTalks);            
        }

        private void AdjustListView<T>(NonScrollableListView listView, IEnumerable<T> items, int androidAdditionalHeight = 1)
        {
            var count = items?.Count() ?? 0;
            if (count <= 0)
            {
                return;
            }

            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -count + androidAdditionalHeight;
            listView.HeightRequest = count * listView.RowHeight - adjust;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ListViewSeeAlsoTalks.ItemTapped -= ListViewTapped;
            ListViewSpeakers.ItemTapped -= ListViewTapped;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.talkViewModel = null;

            ListViewSpeakers.HeightRequest = 0;
            ListViewSeeAlsoTalks.HeightRequest = 0;

            AdjustListView(ListViewSessionMaterial, ViewModel.SessionMaterialItems, androidAdditionalHeight: 2);
        }

        private void Cell_OnAppearing(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            this.ListViewSeeAlsoTalks.AdjustHeight(viewCell);
        }
    }
}
