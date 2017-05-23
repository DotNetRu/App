using System;
using System.Linq;
using UIKit;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Utils;

namespace XamarinEvolve.iOS.UpNext
{
	public partial class FavoriteSessionCell : UITableViewCell
	{
		private const string DEFAULT_COLOR = "5ac8fa"; // light blue

		public FavoriteSessionCell(IntPtr handle) : base(handle)
		{
		}

		public void UpdateCell(Session session)
		{
            try
            {
                TitleLabel.Text = session.Title;
                RoomLabel.Text = session.Room?.Name ?? "Room TBD";
                TimeLabel.Text = session.StartTime.Value.ToEventTimeZone().ToString("HH:mm");
				EndTimeLabel.Text = session.EndTime.Value.ToEventTimeZone().ToString("HH:mm");

                var category = session.Categories?.FirstOrDefault();

                var color = UIColor.Clear.FromHexString(category?.Color ?? DEFAULT_COLOR);

                if (color == UIColor.White)
                {
                    color = UIColor.Clear.FromHexString(DEFAULT_COLOR);
                }
                DividerView.BackgroundColor = color;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
		}
	}
}
