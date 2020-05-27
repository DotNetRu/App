namespace DotNetRu.Clients.UI.Cells
{
    using System;
    using Xamarin.Forms;

    public partial class SubscriptionCell
    {
        public SubscriptionCell()
        {
            this.InitializeComponent();
        }

        private void LoadedPosts_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (sender is Slider slider)
            {
                var newStep = Math.Round(e.NewValue / 5.0d);
                slider.Value = newStep * 5.0d;
            }
        }
    }
}
