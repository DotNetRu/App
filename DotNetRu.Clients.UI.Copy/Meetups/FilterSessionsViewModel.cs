namespace DotNetRu.Clients.Portable.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using DotNetRu.AppUtils;
    using DotNetRu.DataStore.Audit.Models;
    using Microsoft.Maui;

    public class FilterSessionsViewModel : ViewModelBase
    {
        public FilterSessionsViewModel(INavigation navigation)
            : base(navigation)
        {
            this.AllCategory = new Category { Name = "All" };
        }

        public Category AllCategory { get; }

        public List<Category> Categories { get; } = new List<Category>();

        public void LoadCategories()
        {
            this.Categories.Clear();
            var items = new List<Category>();

            foreach (var category in items.OrderBy(c => c.Name))
            {
                this.Categories.Add(category);
            }

            this.Save();
        }


        public void Save()
        {
            Settings.FilteredCategories = string.Join(
                "|",
                this.Categories?.Where(c => c.IsFiltered).Select(c => c.Name));
        }
    }
}

