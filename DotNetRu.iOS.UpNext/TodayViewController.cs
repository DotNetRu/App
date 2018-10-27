using System;
using System.Collections.Generic;
using System.Linq;

using CoreGraphics;

using Foundation;

using MvvmHelpers;

using NotificationCenter;

using UIKit;

namespace UpNext
{
    using DotNetRu.DataStore.Audit.Models;

    using XamarinEvolve.Utils.Helpers;

    public partial class TodayViewController : UIViewController, INCWidgetProviding
    {
        private IEnumerable<Grouping<string, TalkModel>> _data;

        private CGSize _collapsedSize;

        protected TodayViewController(IntPtr handle)
            : base(handle)
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

            this.ExtensionContext.SetWidgetLargestAvailableDisplayMode(NCWidgetDisplayMode.Expanded);

            // Get the possible sizes
            this._collapsedSize = this.ExtensionContext.GetWidgetMaximumSize(NCWidgetDisplayMode.Compact);

            if (this.IsInitialized())
            {
            }
            else
            {
                this.MainTitleLabel.Text = "Please open the app and load the sessions list once to initialize.";
                this.MainTitleLabel.Hidden = false;
                this.SessionsTable.Hidden = true;
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
            this.SetPreferredContentSize();
        }

        [Export("widgetPerformUpdateWithCompletionHandler:")]
        public async void WidgetPerformUpdate(Action<NCUpdateResult> completionHandler)
        {
            // If an error is encoutered, use NCUpdateResultFailed
            // If there's no update required, use NCUpdateResultNoData
            // If there's an update, use NCUpdateResultNewData
            if (this.IsInitialized())
            {
                try
                {
                    this._data = null;

                    if (this._data?.Any() ?? false)
                    {
                        this.MainTitleLabel.Hidden = true;
                        this.SessionsTable.Hidden = false;

                        this.SessionsTable.RowHeight = UITableView.AutomaticDimension;
                        this.SessionsTable.EstimatedRowHeight = 65;
                        this.SessionsTable.ReloadData();
                    }
                    else
                    {
                        this.MainTitleLabel.Hidden = false;
                        this.MainTitleLabel.Text = "You have no upcoming favorites";
                        this.SessionsTable.Hidden = true;
                    }

                    this.SetPreferredContentSize();

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
            if (this.ExtensionContext.GetWidgetActiveDisplayMode() == NCWidgetDisplayMode.Compact)
            {
                this.PreferredContentSize = this._collapsedSize;
            }
            else
            {
                var height = (!this._data?.Any() ?? true)
                                 ? 100
                                 : (this._data.Count() * this.SessionsTable.SectionHeaderHeight)
                                   + (this._data.SelectMany(g => g.AsEnumerable()).Count()
                                      * this.SessionsTable.EstimatedRowHeight) + this.SessionsTable.SectionFooterHeight + 70;
                Console.WriteLine($"Requesting widget height: {height}");
                this.PreferredContentSize = new CGSize(0, height);
            }
        }
    }
}
