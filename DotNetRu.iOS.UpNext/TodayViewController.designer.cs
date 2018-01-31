// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;
using UIKit;

namespace DotNetRu.iOS.UpNext
{
    [Register ("TodayViewController")]
	partial class TodayViewController
	{
		[Outlet]
		UILabel MainTitleLabel { get; set; }

		[Outlet]
		UITableView SessionsTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MainTitleLabel != null) {
				MainTitleLabel.Dispose ();
				MainTitleLabel = null;
			}

			if (SessionsTable != null) {
				SessionsTable.Dispose ();
				SessionsTable = null;
			}
		}
	}
}
