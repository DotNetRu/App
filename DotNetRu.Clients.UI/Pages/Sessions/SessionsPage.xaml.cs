namespace XamarinEvolve.Clients.UI
{
    using System;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataObjects;

    public partial class SessionsPage : BasePage
    {
        public override AppPage PageType => AppPage.Sessions;

        SessionsViewModel ViewModel => vm ?? (vm = BindingContext as SessionsViewModel);

        SessionsViewModel vm;

        bool showPast;

        bool showAllCategories;

        string filteredCategories;

        ToolbarItem filterItem;

        string loggedIn;

        public SessionsPage()
        {
            InitializeComponent();
            showPast = Settings.Current.ShowPastSessions;
            showAllCategories = Settings.Current.ShowAllCategories;
            filteredCategories = Settings.Current.FilteredCategories;

            BindingContext = vm = new SessionsViewModel(Navigation);

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItems.Add(
                    new ToolbarItem
                        {
                            Text = "Refresh",
                            Icon = "toolbar_refresh.png",
                            Command = vm.ForceRefreshCommand
                        });
            }

            filterItem = new ToolbarItem { Text = "Filter" };

            if (Device.OS != TargetPlatform.iOS) filterItem.Icon = "toolbar_filter.png";

            filterItem.Command = new Command(
                async () =>
                    {
                        if (vm.IsBusy) return;
                        await NavigationService.PushModalAsync(
                            Navigation,
                            new EvolveNavigationPage(new FilterSessionsPage()));
                    });

            ToolbarItems.Add(filterItem);

            ListViewSessions.ItemSelected += async (sender, e) =>
                {
                    var session = ListViewSessions.SelectedItem as Session;
                    if (session == null) return;

                    var sessionDetails = new SessionDetailsPage(session);

                    await NavigationService.PushAsync(Navigation, sessionDetails);
                    ListViewSessions.SelectedItem = null;
                };
        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null) return;
            list.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ListViewSessions.ItemTapped += ListViewTapped;

            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Subscribe("filter_changed", (d) => UpdatePage());

            UpdatePage();
        }

        void UpdatePage()
        {
            Title = "Sessions";

            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow));

            // Load if none, or if 45 minutes has gone by
            if ((ViewModel?.Sessions?.Count ?? 0) == 0 || forceRefresh)
            {
                ViewModel?.LoadSessionsCommand?.Execute(forceRefresh);
            }
            else if (showPast != Settings.Current.ShowPastSessions
                     || showAllCategories != Settings.Current.ShowAllCategories
                     || filteredCategories != Settings.Current.FilteredCategories)
            {
                showPast = Settings.Current.ShowPastSessions;
                showAllCategories = Settings.Current.ShowAllCategories;
                filteredCategories = Settings.Current.FilteredCategories;
                ViewModel?.FilterSessionsCommand?.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ListViewSessions.ItemTapped -= ListViewTapped;
            if (Device.OS == TargetPlatform.Android) MessagingService.Current.Unsubscribe("filter_changed");
        }

        public void OnResume()
        {
            UpdatePage();
        }
    }
}