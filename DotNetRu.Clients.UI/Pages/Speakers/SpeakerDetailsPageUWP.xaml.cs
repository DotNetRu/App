﻿namespace DotNetRu.Clients.UI.Pages.Speakers
{
    using System.Linq;

    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Sessions;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public partial class SpeakerDetailsPageUWP
    {
        public override AppPage PageType => AppPage.Speaker;

        SpeakerDetailsViewModel ViewModel => this.vm ?? (this.vm = this.BindingContext as SpeakerDetailsViewModel);

        SpeakerDetailsViewModel vm;

        public SpeakerDetailsPageUWP(SpeakerModel speakerModel)
            : this((string)null)
        {
            this.SpeakerModel = speakerModel;
        }

        public SpeakerDetailsPageUWP(string sessionId)
        {
            this.InitializeComponent();

            this.ListViewSessions.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewSessions.SelectedItem is TalkModel session))
                    {
                        return;
                    }

                    var sessionDetails = new TalkPage(session);

                    await NavigationService.PushAsync(this.Navigation, sessionDetails);

                    this.ListViewSessions.SelectedItem = null;
                };
        }

        public SpeakerModel SpeakerModel
        {
            get => this.ViewModel.SpeakerModel;
            set
            {
                this.BindingContext = new SpeakerDetailsViewModel(value);
                this.ItemId = value?.FullName;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.vm = null;

            this.ListViewFollow.HeightRequest = (this.ViewModel.FollowItems.Count * this.ListViewFollow.RowHeight) - 1;
            this.ListViewSessions.HeightRequest = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewFollow.ItemTapped += this.ListViewTapped;
            this.ListViewSessions.ItemTapped += this.ListViewTapped;

            this.ListViewSessions.HeightRequest = (this.ViewModel.Talks.Count() * this.ListViewSessions.RowHeight) - 1;
        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
                return;
            list.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewFollow.ItemTapped -= this.ListViewTapped;
            this.ListViewSessions.ItemTapped -= this.ListViewTapped;
        }
    }
}
