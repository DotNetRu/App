namespace DotNetRu.Clients.UI.Pages.Info
{
    using DotNetRu.Clients.Portable.ViewModel;

    public partial class TechnologiesUsedPage
    {
        public TechnologiesUsedPage()
        {
            InitializeComponent();
            this.BindingContext = new TechnologiesUsedViewModel();
            this.ListViewTechnology.ItemTapped += (sender, e) => this.ListViewTechnology.SelectedItem = null;
        }
    }
}