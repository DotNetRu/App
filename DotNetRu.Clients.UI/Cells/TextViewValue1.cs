using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Cells
{
    public class TextViewValue1 : TextCell
    {
        public TextViewValue1()
        {
            this.DetailColor = (Color) Application.Current.Resources["DetailTextColor"];
        }
    }
}

