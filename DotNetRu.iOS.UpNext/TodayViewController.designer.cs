// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using System.CodeDom.Compiler;

using Foundation;

namespace UpNext
{
    using UIKit;

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
