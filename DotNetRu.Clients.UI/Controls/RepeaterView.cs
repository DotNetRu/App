﻿namespace DotNetRu.Clients.UI.Controls
{
    using System.Collections.ObjectModel;

    using Xamarin.Forms;

    public class RepeaterView<T> : StackLayout
    {
        public RepeaterView()
        {
            this.Spacing = 0;
        }

        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create<RepeaterView<T>, ObservableCollection<T>>(p => p.ItemsSource, new ObservableCollection<T>(), BindingMode.OneWay, null, ItemsChanged);

        private static void ItemsChanged(BindableObject bindable, ObservableCollection<T> oldValue, ObservableCollection<T> newValue)
        {
            var control = bindable as RepeaterView<T>;
            control.ItemsSource.CollectionChanged += control.ItemsSource_CollectionChanged;
            control.Children.Clear();

            foreach (var item in newValue)
            {
                var cell = control.ItemTemplate.CreateContent();
                control.Children.Add(((ViewCell)cell).View);
            }

            control.UpdateChildrenLayout();
            control.InvalidateLayout();
        }


        void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                this.Children.RemoveAt(e.OldStartingIndex);
                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            }

            if (e.NewItems != null)
            {
                foreach (T item in e.NewItems)
                {
                    var cell = this.ItemTemplate.CreateContent();
                    var view = ((ViewCell)cell).View;
                    view.BindingContext = item;
                    this.Children.Insert(ItemsSource.IndexOf(item), view);
                }

                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create<RepeaterView<T>, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
    }
}
