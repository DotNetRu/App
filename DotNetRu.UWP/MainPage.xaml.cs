namespace DotNetRu.UWP
{
    using System;
    using System.Diagnostics;

    using Windows.Foundation;
    using Windows.UI.ViewManagement;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            try
            {
                this.LoadApplication(new Clients.UI.App());
            }
            catch (Exception)
            {
                Debugger.Break();
            }

            ApplicationView.PreferredLaunchViewSize = new Size(1024, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(1024, 768));
        }
    }
}
