namespace DotNetRu.Clients.UI.Controls
{
    using System.Collections;

    using Xamarin.Forms;

    public class StackLayoutList : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IList),
            typeof(StackLayoutList),
            propertyChanged: OnItemsSourceChanged);

        public IList ItemsSource
        {
            get => (IList)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(StackLayoutList),
            propertyChanged: OnItemTemplateChanged);

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }

        private static void OnItemTemplateChanged(BindableObject pObj, object pOldVal, object pNewVal)
        {
            if (pObj is StackLayoutList layout && layout.ItemsSource != null)
            {
                layout.BuildLayout();
            }
        }

        private static void OnItemsSourceChanged(BindableObject pObj, object pOldVal, object pNewVal)
        {
            if (pObj is StackLayoutList layout && layout.ItemTemplate != null)
            {
                layout.BuildLayout();
            }
        }

        private void BuildLayout()
        {
            this.Children.Clear();

            foreach (var item in this.ItemsSource)
            {
                var view = (View)this.ItemTemplate.CreateContent();
                view.BindingContext = item;
                this.Children.Add(view);
            }
        }
    }
}