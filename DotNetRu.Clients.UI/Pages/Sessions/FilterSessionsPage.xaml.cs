namespace XamarinEvolve.Clients.UI
{
    using System;

    using FormsToolkit;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils.Helpers;

    public partial class FilterSessionsPage : BasePage
    {
        public override AppPage PageType => AppPage.Filter;

        FilterSessionsViewModel vm;

        Category showPast;

        public FilterSessionsPage()
        {
            InitializeComponent();

            if (Device.OS != TargetPlatform.iOS) ToolbarDone.Icon = "toolbar_close.png";

            ToolbarDone.Command = new Command(
                async () =>
                    {
                        Settings.Current.ShowPastSessions = showPast.IsFiltered;
                        vm.Save();
                        await Navigation.PopModalAsync();
                        if (Device.OS == TargetPlatform.Android) MessagingService.Current.SendMessage("filter_changed");

                    });

            BindingContext = vm = new FilterSessionsViewModel(Navigation);






            LoadCategories();
        }


        void LoadCategories()
        {
            vm.LoadCategoriesAsync().ContinueWith(
                (result) =>
                    {
                        Device.BeginInvokeOnMainThread(
                            () =>
                                {
                                    var allCell = new CategoryCell { BindingContext = vm.AllCategory };

                                    TableSectionCategories.Add(allCell);

                                    foreach (var item in vm.Categories)
                                    {
                                        TableSectionCategories.Add(new CategoryCell { BindingContext = item });
                                    }

                                    var color = Device.OS == TargetPlatform.Windows
                                                || Device.OS == TargetPlatform.WinPhone
                                                    ? ((Color)App.Current.Resources["Primary"]).ToHex()
                                                    : string.Empty;

                                    showPast = new Category
                                                   {
                                                       Name = "Show Past Sessions",
                                                       IsEnabled = true,
                                                       ShortName = "Show Past Sessions",
                                                       Color = color
                                                   };

                                    TableSectionFilters.Add(new CategoryCell { BindingContext = showPast });

                                    //if end of evolve
                                    if (DateTime.UtcNow > EventInfo.EndOfConference) showPast.IsEnabled = false;

                                    showPast.IsFiltered = Settings.Current.ShowPastSessions;


                                });
                    });
        }
    }
}

