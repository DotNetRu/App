using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using XamarinEvolve.DataObjects;
using MvvmHelpers;
using CoreGraphics;
using XamarinEvolve.Utils;

namespace XamarinEvolve.iOS.UpNext
{
    public class FavoriteSessionsTableViewSource: UITableViewSource
	{
		const string REUSE_IDENTIFIER = "FavoriteSessionCell";

        IEnumerable<Grouping<string, Session>> _groupedSessions;
        NSExtensionContext _context;

		public FavoriteSessionsTableViewSource(IEnumerable<Grouping<string, Session>> groupedSessions, NSExtensionContext context)
		{
            _groupedSessions = groupedSessions;
            _context = context;
		}

        public override nint NumberOfSections(UITableView tableView)
        {
            return _groupedSessions.Count();
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return "  " + _groupedSessions.ElementAt((int) section).Key;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (FavoriteSessionCell) tableView.DequeueReusableCell(REUSE_IDENTIFIER, indexPath);

            var item = _groupedSessions.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
			cell.UpdateCell(item);

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return _groupedSessions.ElementAt((int) section).Count();
		}

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            var session = _groupedSessions.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
            _context.OpenUrl(NSUrl.FromString($"{AboutThisApp.AppLinkProtocol}:{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory.ToLowerInvariant()}/{session.Id}"), null);


        }
    }
}
