namespace DotNetRu.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // DependencyService.Register<ILogger, DotNetRuLogger>();

            MainPage = new AppShell();
        }
    }
}
