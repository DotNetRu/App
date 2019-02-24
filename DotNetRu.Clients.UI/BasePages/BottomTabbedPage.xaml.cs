namespace DotNetRu.Clients.UI.Pages
{
    using System;
    using DotNetRu.Clients.Portable.ViewModel;
    using Xamarin.Essentials;
    using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

    public partial class BottomTabbedPage
    {
        public BottomTabbedPage()
        {
            this.InitializeComponent();

            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            this.BindingContext = new BottomBarViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var pushReceivedTime = Preferences.Get("PushReceived", DateTime.MinValue);

            DisplayAlert("Alert", pushReceivedTime.ToLongTimeString(), "OK");
        }
    }
}
