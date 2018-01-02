using System;
using DotNetRu.Clients.UI.Cells;
using DotNetRu.iOS.Renderers;
using FormsToolkit.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(TextViewValue1), typeof(TextViewValue1Renderer))]
namespace DotNetRu.iOS.Renderers
{
    public class TextViewValue1Renderer : TextCellRenderer
    {
        public static void Init()
        {
            var test = DateTime.UtcNow;
        }

        public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
        {

            var tvc = reusableCell as CellTableViewCell;
            if (tvc == null) {
                tvc = new CellTableViewCell (UITableViewCellStyle.Value1, item.GetType().FullName);
            }

            tvc.Cell = item;
            var cell = base.GetCell(item, tvc, tv);
            cell.SetDisclosure(item.StyleId);
            return cell;
        }
    }
}

