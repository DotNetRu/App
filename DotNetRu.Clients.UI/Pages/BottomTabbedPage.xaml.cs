using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Clients.Portable.ViewModel;

namespace XamarinEvolve.Clients.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomTabbedPage
    {
        public BottomTabbedPage()
        {
            InitializeComponent();
            this.BindingContext = new BottomBarViewModel();
        }
    }
}