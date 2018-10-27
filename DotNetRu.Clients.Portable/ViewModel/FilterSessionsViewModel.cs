using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinEvolve.Clients.Portable
{
    using DotNetRu.DataStore.Audit.Models;

    public class FilterSessionsViewModel : ViewModelBase
    {
        public FilterSessionsViewModel(INavigation navigation)
            : base(navigation)
        {
            this.AllCategory = new Category
                {
                    Name = "All",
                    IsEnabled = true,
                    IsFiltered = this.Settings.ShowAllCategories
                };

            this.AllCategory.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsFiltered") this.SetShowAllCategories(this.AllCategory.IsFiltered);
            };
              
        }


        public Category AllCategory { get; }
        public List<Category> Categories { get; } = new List<Category>();

        private void SetShowAllCategories(bool showAll)
        {
			// first save changes to individual filters
            this.Save();
            this.Settings.ShowAllCategories = showAll;
            foreach(var category in this.Categories)
            {
                category.IsEnabled = !this.Settings.ShowAllCategories;
                category.IsFiltered = this.Settings.ShowAllCategories || this.Settings.FilteredCategories.Contains(category.Name);
            }
        }

        public async Task LoadCategoriesAsync()
        {
            this.Categories.Clear();
            var items = await this.StoreManager.CategoryStore.GetItemsAsync();
            try 
            {
                if (!items.Any ())
                    items = await this.StoreManager.CategoryStore.GetItemsAsync (true);
            } 
            catch 
            {
                items = await this.StoreManager.CategoryStore.GetItemsAsync (true);
            }
            
            foreach(var category in items.OrderBy(c => c.Name))
            {
                category.IsFiltered = this.Settings.ShowAllCategories || this.Settings.FilteredCategories.Contains(category.Name); 
                category.IsEnabled = !this.Settings.ShowAllCategories;
                this.Categories.Add(category);
            }

            this.Save();
        }

       
        public void Save()
        {
			if (!this.Settings.ShowAllCategories)
			{
			    this.Settings.FilteredCategories = string.Join("|", this.Categories?.Where(c => c.IsFiltered).Select(c => c.Name));
			}
        }
    }
}

