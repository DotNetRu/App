namespace DotNetRu.Clients.UI.Pages.Info
{
    using System;

    using DotNetRu.Clients.Portable.ViewModel;
    using DotNetRu.Utils.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TechnologiesUsedPage
    {
        readonly TechnologiesUsedViewModel technologiesUsedViewModel;

        public TechnologiesUsedPage()
        {
            InitializeComponent();
            this.BindingContext = this.technologiesUsedViewModel = new TechnologiesUsedViewModel();
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.technologiesUsedViewModel.TechnologyItems.Count + 1;
            this.ListViewTechnology.HeightRequest =
                (this.technologiesUsedViewModel.TechnologyItems.Count * this.ListViewTechnology.RowHeight) - adjust;
            this.ListViewTechnology.ItemTapped += (sender, e) => this.ListViewTechnology.SelectedItem = null;
        }


        bool dialogShown;

        int count;

        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            this.count++;
            if (this.dialogShown || this.count < 8)
                return;

            this.dialogShown = true;

            App.Logger.Track("AppCreditsFound");

            await this.DisplayAlert("Credits", AboutThisApp.Credits, "OK");
        }
    }
}