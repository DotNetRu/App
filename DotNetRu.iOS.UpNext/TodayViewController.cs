using System;
using NotificationCenter;
using Foundation;
using UIKit;
using UpNext.Services;
using System.Linq;
using XamarinEvolve.iOS.UpNext;
using System.Collections.Generic;
using XamarinEvolve.DataObjects;
using CoreGraphics;
using MvvmHelpers;
using XamarinEvolve.Utils;

namespace UpNext
{
	using XamarinEvolve.Utils.Helpers;

	public partial class TodayViewController : UIViewController, INCWidgetProviding
	{
		private IEnumerable<Grouping<string, Session>> _data;
		private CGSize _collapsedSize;

		protected TodayViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ExtensionContext.SetWidgetLargestAvailableDisplayMode(NCWidgetDisplayMode.Expanded);

			// Get the possible sizes
			_collapsedSize = ExtensionContext.GetWidgetMaximumSize(NCWidgetDisplayMode.Compact);

			if (IsInitialized())
			{
				if (!_data?.Any() ?? true)
				{
					MainTitleLabel.Text = "Loading your favorites...";
					MainTitleLabel.Hidden = false;
					SessionsTable.Hidden = true;
				}
			}
			else
			{
				MainTitleLabel.Text = "Please open the app and load the sessions list once to initialize.";
				MainTitleLabel.Hidden = false;
				SessionsTable.Hidden = true;
			}
		}

		bool IsInitialized()
		{
			try
			{
				var settings = new NSUserDefaults($"group.{AboutThisApp.PackageName}", NSUserDefaultsType.SuiteName);
				settings.Synchronize();
				return settings.BoolForKey("FavoritesInitialized");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}

		[Export("widgetActiveDisplayModeDidChange:withMaximumSize:")]
		public void WidgetActiveDisplayModeDidChange(NCWidgetDisplayMode activeDisplayMode, CGSize maxSize)
		{
			SetPreferredContentSize();
		}

		[Export("widgetPerformUpdateWithCompletionHandler:")]
		public async void WidgetPerformUpdate(Action<NCUpdateResult> completionHandler)
		{
			// If an error is encoutered, use NCUpdateResultFailed
			// If there's no update required, use NCUpdateResultNoData
			// If there's an update, use NCUpdateResultNewData

			if (IsInitialized())
			{
				try
				{
					_data = await FavoriteService.GetFavorites();

					if (_data?.Any() ?? false)
					{
						MainTitleLabel.Hidden = true;
						SessionsTable.Hidden = false;

						SessionsTable.RowHeight = UITableView.AutomaticDimension;
						SessionsTable.EstimatedRowHeight = 65;
						SessionsTable.Source = new FavoriteSessionsTableViewSource(_data, ExtensionContext);
						SessionsTable.ReloadData();
					}
					else
					{
						MainTitleLabel.Hidden = false;
						MainTitleLabel.Text = "You have no upcoming favorites";
						SessionsTable.Hidden = true;
					}

					SetPreferredContentSize();

					completionHandler(NCUpdateResult.NewData);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					completionHandler(NCUpdateResult.Failed);
				}
			}
			else
			{
				completionHandler(NCUpdateResult.NoData);
			}
		}

		private void SetPreferredContentSize()
		{
			if (ExtensionContext.GetWidgetActiveDisplayMode() == NCWidgetDisplayMode.Compact)
			{
				PreferredContentSize = _collapsedSize;
			}
			else
			{
				var height = (!_data?.Any() ?? true) ? 100 : (_data.Count() * SessionsTable.SectionHeaderHeight) +
					(_data.SelectMany(g => g.AsEnumerable()).Count() * SessionsTable.EstimatedRowHeight) +
					SessionsTable.SectionFooterHeight + 70;
				Console.WriteLine($"Requesting widget height: {height}");
				PreferredContentSize = new CGSize(0, height);
			}
		}
	}
}
