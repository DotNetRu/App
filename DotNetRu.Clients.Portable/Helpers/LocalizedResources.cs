namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;

    using Xamarin.Forms;

    using XamarinEvolve.Clients.Portable;
    using XamarinEvolve.Utils.Helpers;

    public class LocalizedResources : INotifyPropertyChanged
    {
        private readonly ResourceManager resourceManager;

        private CultureInfo currentCultureInfo;

        public LocalizedResources(Type resource, Language language)
            : this(resource, new CultureInfo(language.GetLanguageCode()))
        {
        }

        public LocalizedResources(Type resource, CultureInfo cultureInfo)
        {
            this.currentCultureInfo = cultureInfo;
            this.resourceManager = new ResourceManager(resource);

            MessagingCenter.Subscribe<object, CultureChangedMessage>(this, string.Empty, this.OnCultureChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key] => this.resourceManager.GetString(key, this.currentCultureInfo);

        private void OnCultureChanged(object s, CultureChangedMessage ccm)
        {
            this.currentCultureInfo = ccm.NewCultureInfo;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
            MessagingCenter.Send(this, MessageKeys.LanguageChanged);
        }
    }
}
