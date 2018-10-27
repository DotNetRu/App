namespace XamarinEvolve.Clients.UI
{
    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;

    public partial class FilterSessionsPage : BasePage
    {
        public override AppPage PageType => AppPage.Filter;

        private readonly FilterSessionsViewModel filterSessionsViewModel;

        public FilterSessionsPage()
        {
            this.InitializeComponent();

            if (Device.RuntimePlatform != Device.iOS)
            {
                this.ToolbarDone.Icon = "toolbar_close.png";
            }

            this.ToolbarDone.Command = new Command(
                async () =>
                    {
                        this.filterSessionsViewModel.Save();
                        await this.Navigation.PopModalAsync();
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            MessagingService.Current.SendMessage("filter_changed");
                        }
                    });

            this.BindingContext = this.filterSessionsViewModel = new FilterSessionsViewModel(this.Navigation);

            this.LoadCategories();
        }


        void LoadCategories()
        {
            this.filterSessionsViewModel.LoadCategoriesAsync().ContinueWith(
                (result) =>
                    {
                        Device.BeginInvokeOnMainThread(
                            () =>
                                {
                                    var allCell =
                                        new CategoryCell { BindingContext = this.filterSessionsViewModel.AllCategory };

                                    this.TableSectionCategories.Add(allCell);

                                    foreach (var item in this.filterSessionsViewModel.Categories)
                                    {
                                        this.TableSectionCategories.Add(new CategoryCell { BindingContext = item });
                                    }
                                });
                    });
        }
    }
}

