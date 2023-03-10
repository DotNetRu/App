using System.Linq;
using DotNetRu.AppUtils;
using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.UI.Localization;
using Microsoft.Maui;
using Xamarin.Forms.Internals;

namespace DotNetRu.Clients.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();

            var primaryColor = (Color)Application.Current.Resources["Primary"];

            var backgroundColor = Device.RuntimePlatform == Device.iOS ? Color.White : primaryColor;

            // Can't set BackgrounColor as StaticResource, see https://github.com/xamarin/Xamarin.Forms/issues/7055
            SetBackgroundColor(this, backgroundColor);

            MessagingCenter.Subscribe<LocalizedResources>(this, MessageKeys.LanguageChanged, sender => this.UpdateTabBarLocalization());
        }

        public void UpdateTabBarLocalization()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.TabBar.Items.Where(i => i is LocalizableTab).Cast<LocalizableTab>().ForEach(x => x.Update());
            });
        }
    }
}
