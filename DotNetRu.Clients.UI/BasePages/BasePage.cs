namespace DotNetRu.Clients.UI.Pages
{
    using System.Diagnostics;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Clients.Portable.Model;

    public abstract class BasePage : ContentPage, IProvidePageInfo
    {
        private Stopwatch appeared;

        public abstract AppPage PageType { get; }

        protected string ItemId { get; set; }

        protected override void OnAppearing()
        {
            this.appeared = Stopwatch.StartNew();
            App.Logger.TrackPage(this.PageType.ToString(), this.ItemId);

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            App.Logger.TrackTimeSpent(this.PageType.ToString(), this.ItemId, TimeSpan.FromTicks(DateTime.UtcNow.Ticks).Subtract(this.appeared.Elapsed));
            base.OnDisappearing();
        }
    }
}
