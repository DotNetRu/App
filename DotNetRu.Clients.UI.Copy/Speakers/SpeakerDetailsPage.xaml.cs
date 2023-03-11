using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Sessions;
using DotNetRu.Clients.UI.Pages.Speakers;

namespace DotNetRu.Clients.UI.Speakers
{
    public partial class SpeakerDetailsPage
    {
        public SpeakerDetailsPage()
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

            this.ListViewFollow.TemplatedItems.CollectionChanged += (sender, args) =>
                {
                    this.ListViewFollow.UpdateListViewHeight();
                };
        }

        private SpeakerDetailsViewModel speakerDetailsViewModel;

        public SpeakerDetailsPage(SpeakerModel speakerModel)
            : this()
        {
            this.SpeakerModel = speakerModel;

            this.speakerImage.Error += (source, arg) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.speakerFullNameLabel.FontSize *= 2;
                    this.speakerTitleLabel.FontSize *= 2;
                });
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
                this.ItemId = value.Id;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.speakerDetailsViewModel = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewFollow.ItemTapped -= this.ListViewTapped;
            this.ListViewSessions.ItemTapped -= this.ListViewTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.ListViewFollow.ItemTapped += this.ListViewTapped;
            this.ListViewSessions.ItemTapped += this.ListViewTapped;
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
            await NavigationService.PushAsync(this.Navigation, new SpeakerFacePage(this.SpeakerModel.Id, this.SpeakerModel.AvatarURL));
        }
    }
}
