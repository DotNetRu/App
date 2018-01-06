using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Friends;
using DotNetRu.Clients.UI.Pages.Info;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using MenuItem = DotNetRu.Clients.Portable.Model.MenuItem;

namespace DotNetRu.Clients.UI.Pages.iOS
{
    using MenuItem = MenuItem;

    public partial class AboutPage
    {
        private readonly AboutViewModel aboutViewModel;

        private readonly IPushNotifications push;

        private bool isPushRegistered = false;

        public AboutPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.aboutViewModel = new AboutViewModel();
            this.push = DependencyService.Get<IPushNotifications>();
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.aboutViewModel.AboutItems.Count + 1;
            this.ListViewAbout.HeightRequest = (this.aboutViewModel.AboutItems.Count * this.ListViewAbout.RowHeight) - adjust;
            this.ListViewAbout.ItemTapped += (sender, e) => this.ListViewAbout.SelectedItem = null;
            this.ListViewInfo.HeightRequest = (this.aboutViewModel.InfoItems.Count * this.ListViewInfo.RowHeight) - adjust;

            this.ListViewAbout.ItemSelected += async (sender, e) =>
                {
                    if (this.ListViewAbout.SelectedItem == null)
                    {
                        return;
                    }

                    await NavigationService.PushAsync(this.Navigation, new SettingsPage());

                    this.ListViewAbout.SelectedItem = null;
                };

            this.ListViewInfo.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewInfo.SelectedItem is MenuItem item))
                    {
                        return;
                    }
                    Page page = null;
                    switch (item.Parameter)
                    {
                        case "sponsors":
                            page = new FriendsPage();
                            break;
                    }

                    if (page == null) return;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await NavigationService.PushAsync(((Page)this.Parent.Parent).Navigation, page);
                    }
                    else
                    {
                        await NavigationService.PushAsync(this.Navigation, page);
                    }

                    this.ListViewInfo.SelectedItem = null;
                };
            //this.isPushRegistered = this.push.IsRegistered;
        }

        public override AppPage PageType => AppPage.Information;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!this.isPushRegistered && Settings.Current.AttemptedPush)
            {
                this.push.RegisterForNotifications();
            }

            //this.isPushRegistered = this.push.IsRegistered;
        }

        public void OnResume()
        {
            this.OnAppearing();
        }
    }
}

