using System;
using System.Diagnostics;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages.Home
{
	public partial class TweetImagePage : BasePage
	{
		public override AppPage PageType => AppPage.TweetImage;

        public TweetImagePage(string image)
        {
            this.InitializeComponent();
            var item = new ToolbarItem
            {
                Text = "Done",
                Command = new Command(async () => await this.Navigation.PopModalAsync())
            };
            if (Device.RuntimePlatform == Device.Android)
                item.Icon = "toolbar_close.png";
            this.ToolbarItems.Add(item);

            try
            {
                this.MainImage.Source = new UriImageSource
                {
                    Uri = new Uri(image),
                    CachingEnabled = true,
                    CacheValidity = TimeSpan.FromDays(3)
                };
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to convert image to URI: " + ex);
                DependencyService.Get<IToast>().SendToast("Unable to load image.");
            }

            this.MainImage.PropertyChanged += (sender, e) => 
                {
                    if(e.PropertyName != nameof(this.MainImage.IsLoading))
                        return;
                    this.ProgressBar.IsRunning = this.MainImage.IsLoading;
                    this.ProgressBar.IsVisible = this.MainImage.IsLoading;
                };
        }
	}
}

