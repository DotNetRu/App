using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinEvolve.Clients.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomTabbedPage : Naxam.Controls.Forms.BottomTabbedPage
    {
        public BottomTabbedPage()
        {
            InitializeComponent();
        }
    }
}