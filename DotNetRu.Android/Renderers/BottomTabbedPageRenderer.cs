using DotNetRu.Droid.Renderers;
using Naxam.Controls.Platform.Droid;
using Naxam.Controls.Platform.Droid.Utils;
using Xamarin.Forms;
using XamarinEvolve.Utils.Helpers;
using BottomTabbedPage = DotNetRu.Clients.UI.Pages.BottomTabbedPage;
using SettingsPage = DotNetRu.Clients.UI.Pages.Info.SettingsPage;


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