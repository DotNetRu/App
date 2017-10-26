using System;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	public abstract class BasePage : ContentPage, IProvidePageInfo
	{
		private DateTime _appeared;

		public abstract AppPage PageType { get; }
		protected string ItemId { get; set; }

		protected override void OnAppearing()
		{
		    this._appeared = DateTime.UtcNow;
			App.Logger.TrackPage(this.PageType.ToString(), this.ItemId);

			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			App.Logger.TrackTimeSpent(this.PageType.ToString(), this.ItemId, DateTime.UtcNow - this._appeared);
			base.OnDisappearing();
		}
	}
}

