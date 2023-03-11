using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Cells
{
    public class EvolveGroupHeader : ViewCell
    {
        public EvolveGroupHeader()
        {
            this.View = new EvolveGroupHeaderView();
        }
    }
    public partial class EvolveGroupHeaderView
    {
        public EvolveGroupHeaderView()
        {
            this.InitializeComponent();
        }
    }
}

