

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DotNetRu.UWP
{
    using System;
    using System.Diagnostics;

    using DotNetRu.Clients.UI.Pages.Windows;

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

            RootPageWindows.IsDesktop = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop";
            try
            {
                LoadApplication(new Clients.UI.App());
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
