

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DotNetRu.UWP
{
    using System;
    using System.Diagnostics;

    using Windows.Foundation;
    using Windows.System.Profile;
    using Windows.UI.ViewManagement;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
      
        public MainPage()
        {
            InitializeComponent();

            XamarinEvolve.Clients.UI.RootPageWindows.IsDesktop = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop";
            try
            {
                ZXing.Net.Mobile.Forms.WindowsUniversal.ZXingScannerViewRenderer.Init();
                LoadApplication(new XamarinEvolve.Clients.UI.App());
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
            ApplicationView.PreferredLaunchViewSize = new Size(1024, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(1024, 768));
        }
    }
}
