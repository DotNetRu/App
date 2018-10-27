namespace XamarinEvolve.Clients.UI
{
    using System;

    using DotNetRu.DataStore.Audit.Models;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public partial class MeetupPage
    {
        public override AppPage PageType => AppPage.Sessions;

        private MeetupViewModel ViewModel =>
            this.meetupViewModel ?? (this.meetupViewModel = this.BindingContext as MeetupViewModel);

        private MeetupViewModel meetupViewModel;

        public MeetupPage(FeaturedEvent meetup = null)
        {
            this.InitializeComponent();

            this.BindingContext = this.meetupViewModel = new MeetupViewModel(this.Navigation, meetup);

            if (Device.RuntimePlatform == Device.UWP)
            {
                this.ToolbarItems.Add(
                    new ToolbarItem
                        {
                            Text = "Refresh",
                            Icon = "toolbar_refresh.png",
                            Command = this.meetupViewModel.ForceRefreshCommand
                        });
            }

            // this.filterItem = new ToolbarItem { Text = "Filter" };

            // if (Device.RuntimePlatform != Device.iOS)
            // {
            // this.filterItem.Icon = "toolbar_filter.png";
            // }

            // this.filterItem.Command = new Command(
            // async () =>
            // {
            // if (this.meetupViewModel.IsBusy) return;
            // await NavigationService.PushModalAsync(
            // this.Navigation,
            // new EvolveNavigationPage(new FilterSessionsPage()));
            // });

            // this.ToolbarItems.Add(this.filterItem);
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

            this.ListViewSessions.ItemTapped += this.ListViewTapped;

            if (Device.RuntimePlatform == Device.Android)
            {
                MessagingService.Current.Subscribe("filter_changed", (d) => this.UpdatePage());
            }

            this.UpdatePage();
        }

        private void UpdatePage()
        {
            bool forceRefresh = DateTime.UtcNow > (this.ViewModel?.NextForceRefresh ?? DateTime.UtcNow);

            // Load if none, or if 45 minutes has gone by
            if ((this.ViewModel?.Sessions?.Count ?? 0) == 0 || forceRefresh)
            {
                this.ViewModel?.LoadSessionsCommand?.Execute(forceRefresh);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.ListViewSessions.ItemTapped -= this.ListViewTapped;
            if (Device.RuntimePlatform == Device.Android)
            {
                MessagingService.Current.Unsubscribe("filter_changed");
            }
        }

        public void OnResume()
        {
            this.UpdatePage();
        }
    }
}