using System;

using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Utils.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.About
{
    public partial class AboutPage
    {
        public AboutPage()
        {            
            this.InitializeComponent();

            var settingsViewModel = new AboutViewModel(this.Navigation);

            this.BindingContext = settingsViewModel;
        }

        public override AppPage PageType => AppPage.Information;

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Title")
            {
                MessagingCenter.Send(this, MessageKeys.UpdateTitles);
            }
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync(AboutThisApp.DotNetRuLink, BrowserLaunchMode.SystemPreferred);
        }
    }
}
