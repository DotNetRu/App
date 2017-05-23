using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
    public partial class FloorMapPage : BasePage
	{
		public override AppPage PageType => AppPage.FloorMap;

        public FloorMapPage (int floor)
        {
            InitializeComponent ();

            var url = string.Empty;
            var local = string.Empty;
            switch (floor) 
            {
                case 0:
                local = "floor_1.png";
				url = $"{AboutThisApp.CdnUrl}floor_1.png";
                Title = "Floor Maps (1/3)";
                    break;
                case 1:
                local = "floor_2.png";
				url = $"{AboutThisApp.CdnUrl}floor_2.png";
                Title = "Floor Maps (2/3)";
                    break;
                case 2:
                    local = "floor_3.png";
                    url = $"{AboutThisApp.CdnUrl}floor_3.png";
                    Title = "Floor Maps (2/3)";
                    break;
            }

            try 
            {
               MainImage.Source = new FileImageSource 
               {
                   File = local
               };

               //MainImage.Source = new UriImageSource 
               //{
               //    Uri = new Uri (url),
               //    CachingEnabled = true,
               //    CacheValidity = TimeSpan.FromDays (3)
               //};
            } 
            catch (Exception ex) 
            {
                Debug.WriteLine ("Unable to convert image to URI: " + ex);
                DependencyService.Get<IToast> ().SendToast ("Unable to load image.");

            }



            MainImage.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName != nameof (MainImage.IsLoading))
                    return;
                ProgressBar.IsRunning = MainImage.IsLoading;
                ProgressBar.IsVisible = MainImage.IsLoading;
            };
        }
    }
}

