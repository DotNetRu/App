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
			_appeared = DateTime.UtcNow;
			App.Logger.TrackPage(PageType.ToString(), ItemId);

			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			App.Logger.TrackTimeSpent(PageType.ToString(), ItemId, DateTime.UtcNow - _appeared);
			base.OnDisappearing();
		}
	}
}

