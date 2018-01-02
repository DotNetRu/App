﻿using System;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Utils.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DotNetRu.Clients.UI.Pages.Info
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TechnologiesUsed : ContentPage
    {
        readonly TechnologiesUsedViewModel vm;

        public TechnologiesUsed()
        {
            InitializeComponent();
            this.BindingContext = this.vm = new TechnologiesUsedViewModel();
            var adjust = Device.RuntimePlatform != Device.Android ? 1 : -this.vm.TechnologyItems.Count + 1;
            this.ListViewTechnology.HeightRequest = (this.vm.TechnologyItems.Count * this.ListViewTechnology.RowHeight) - adjust;
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

            App.Logger.Track("AppCreditsFound-8MoreThan92");

            await this.DisplayAlert("Credits",
                AboutThisApp.Credits, "OK");
        }
    }
}