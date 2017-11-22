using DotNetRu.Droid.Renderers;
using Naxam.Controls.Forms;
using Naxam.Controls.Platform.Droid;
using Naxam.Controls.Platform.Droid.Utils;
using Xamarin.Forms;
using XamarinEvolve.Clients.UI;
using XamarinEvolve.Utils.Helpers;
using BottomTabbedPage = XamarinEvolve.Clients.UI.Pages.BottomTabbedPage;


[assembly: ExportRenderer(typeof(BottomTabbedPage), typeof(BottomTabbedPageRenderer))]

namespace DotNetRu.Droid.Renderers
{
    public class BottomTabbedPageRenderer : BottomTabbedRenderer
    {
        public BottomTabbedPageRenderer()
        {
            MessagingCenter.Subscribe<SettingsPage>(this, MessageKeys.UpdateTitles, model => { this.HandlePagesChanged(); });
        }
    }
}