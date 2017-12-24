namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable.ApplicationResources;
    using XamarinEvolve.Utils.Helpers;

    public class LocalizedResources : INotifyPropertyChanged
    {
        private readonly ResourceManager resourceManager;

        private CultureInfo currentCultureInfo;

        public LocalizedResources(Type resource)
        {
            this.currentCultureInfo = AppResources.Culture;
            this.resourceManager = new ResourceManager(resource);

            MessagingCenter.Subscribe<object, CultureChangedMessage>(this, string.Empty, this.OnCultureChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key] => this.resourceManager.GetString(key, this.currentCultureInfo);

        private void OnCultureChanged(object s, CultureChangedMessage cultureChangedMessage)
        {
            this.currentCultureInfo = cultureChangedMessage.NewCultureInfo;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
            MessagingCenter.Send(this, MessageKeys.LanguageChanged);
        }
    }
}
