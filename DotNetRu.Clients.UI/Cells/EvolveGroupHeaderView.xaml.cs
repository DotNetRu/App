using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
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

