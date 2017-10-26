using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    using MenuItem = Portable.MenuItem;

    public partial class AboutPage : BasePage
    {
        public override AppPage PageType => AppPage.Information;

        AboutViewModel vm;

        IPushNotifications push;

        public AboutPage()
        {
            this.InitializeComponent();
            this.BindingContext = this.vm = new AboutViewModel();
            this.push = DependencyService.Get<IPushNotifications>();
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.vm.AboutItems.Count + 1;
            this.ListViewAbout.HeightRequest = (this.vm.AboutItems.Count * this.ListViewAbout.RowHeight) - adjust;
            this.ListViewAbout.ItemTapped += (sender, e) => this.ListViewAbout.SelectedItem = null;
            this.ListViewInfo.HeightRequest = (this.vm.InfoItems.Count * this.ListViewInfo.RowHeight) - adjust;

            this.ListViewAbout.ItemSelected += async (sender, e) =>
                {
                    if (this.ListViewAbout.SelectedItem == null) return;

                    await NavigationService.PushAsync(this.Navigation, new SettingsPage());

                    this.ListViewAbout.SelectedItem = null;
                };

            this.ListViewInfo.ItemSelected += async (sender, e) =>
                {
                    if (!(this.ListViewInfo.SelectedItem is MenuItem item)) return;
                    Page page = null;
                    switch (item.Parameter)
                    {
                        case "sponsors":
                            page = new SponsorsPage();
                            break;
                    }

                    if (page == null) return;
                    if (Device.RuntimePlatform == Device.iOS)
                        await NavigationService.PushAsync(((Page)this.Parent.Parent).Navigation, page);
                    else await NavigationService.PushAsync(this.Navigation, page);

                    this.ListViewInfo.SelectedItem = null;
                };
            this.isRegistered = this.push.IsRegistered;
        }

        bool isRegistered;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!this.isRegistered && Settings.Current.AttemptedPush)
            {
                this.push.RegisterForNotifications();
            }

            this.isRegistered = this.push.IsRegistered;
        }

        public void OnResume()
        {
            this.OnAppearing();
        }
    }
}

