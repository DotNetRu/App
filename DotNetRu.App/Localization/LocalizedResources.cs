namespace DotNetRu.Clients.Portable.Helpers
{
    using System.ComponentModel;
    using DotNetRu.App.Localization;
    using DotNetRu.AppUtils;
    using DotNetRu.Clients.Portable.Interfaces;

    public class LocalizedResources : INotifyPropertyChanged
    {
        public LocalizedResources()
        {
            // TODO Subscribe only once
            MessagingCenter.Subscribe<object, CultureChangedMessage>(this, string.Empty, this.OnCultureChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key] => AppResources.ResourceManager.GetString(key, AppResources.Culture);

        private void OnCultureChanged(object s, CultureChangedMessage cultureChangedMessage)
        {
            AppResources.Culture = cultureChangedMessage.NewCultureInfo;

            // set for locale-aware methods
            DependencyService.Get<ILocalize>().SetLocale(cultureChangedMessage.NewCultureInfo);

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
            MessagingCenter.Send(this, MessageKeys.LanguageChanged);
        }
    }
}
