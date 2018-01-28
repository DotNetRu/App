using System;
using DotNetRu.Clients.Portable.Interfaces;
using DotNetRu.Clients.Portable.Model;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Pages
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

