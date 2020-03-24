using DotNetRu.Clients.Portable.Helpers;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Utils.Helpers;
using Xamarin.Forms;

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
                if (this.BindingContext is ViewModelBase viewModel)
                {
                    this.NewsTab.Title = viewModel.Resources["News"] ?? this.NewsTab.Title;
                    this.SpeakersTab.Title = viewModel.Resources["Speakers"] ?? this.SpeakersTab.Title;
                    this.MeetupsTab.Title = viewModel.Resources["Meetups"] ?? this.MeetupsTab.Title;
                    this.AboutTab.Title = viewModel.Resources["About"] ?? this.AboutTab.Title;
                }
            });
        }
    }
}
