using DotNetRu.Clients.UI.Controls;
using DotNetRu.Droid.Renderers;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(WhiteTextCell), typeof(WhiteTextCellCustomRenderer))]

namespace DotNetRu.Droid.Renderers
{
    using System.ComponentModel;

    using Android.Content;
    using Android.Views;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    using Color = Android.Graphics.Color;
    using View = Android.Views.View;

    public class WhiteTextCellCustomRenderer : TextCellRenderer
    {
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);
            cell.SetBackgroundColor(Color.White);

            // cell.SetPadding(0, 15, 0, 15);
            return cell;
        }
    }
}