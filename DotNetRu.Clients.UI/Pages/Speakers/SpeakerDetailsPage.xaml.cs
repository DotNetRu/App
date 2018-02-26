namespace DotNetRu.Clients.UI.Pages.Speakers
{
    using System;

    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Model;
    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Clients.UI.Helpers;
    using DotNetRu.Clients.UI.Pages.Sessions;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public partial class SpeakerDetailsPage
    {
        private readonly IPlatformSpecificExtension<SpeakerModel> extension;

        private SpeakerDetailsViewModel speakerDetailsViewModel;

        public SpeakerDetailsPage(SpeakerModel speakerModel)
            : this()
        {
            this.SpeakerModel = speakerModel;
        }

        public SpeakerDetailsPage()
        {
            this.InitializeComponent();
            this.extension = DependencyService.Get<IPlatformSpecificExtension<SpeakerModel>>();

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

            this.ListViewFollow.TemplatedItems.CollectionChanged += (sender, args) =>
                {
                    this.ListViewFollow.UpdateListViewHeight();
                };
        }

        public override AppPage PageType => AppPage.Speaker;

        public SpeakerDetailsViewModel SpeakerDetailsViewModel =>
            this.speakerDetailsViewModel
            ?? (this.speakerDetailsViewModel = this.BindingContext as SpeakerDetailsViewModel);

        public SpeakerModel SpeakerModel
        {
            get => this.SpeakerDetailsViewModel.SpeakerModel;
            set
            {
                this.BindingContext = new SpeakerDetailsViewModel(value);
                this.ItemId = value?.FullName;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.speakerDetailsViewModel = null;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewFollow.ItemTapped -= this.ListViewTapped;
            this.ListViewSessions.ItemTapped -= this.ListViewTapped;

            if (this.extension != null)
            {
                await this.extension.Finish();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewFollow.ItemTapped += this.ListViewTapped;
            this.ListViewSessions.ItemTapped += this.ListViewTapped;

            if (this.SpeakerDetailsViewModel.Talks?.Count > 0)
            {
                return;
            }

            this.SpeakerDetailsViewModel.ExecuteLoadTalksCommand();

            if (this.extension != null)
            {
                await this.extension.Execute(this.SpeakerDetailsViewModel.SpeakerModel);
            }
        }

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(sender is ListView list))
            {
                return;
            }

            list.SelectedItem = null;
        }
       
        private void Cell_OnAppearing(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            this.ListViewSessions.AdjustHeight(viewCell);
        }

        private async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await NavigationService.PushAsync(Navigation, new SpeakerFacePage(SpeakerModel.PhotoImage));
        } 
    }
}
