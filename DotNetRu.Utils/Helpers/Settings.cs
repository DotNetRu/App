namespace DotNetRu.Utils.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Xamarin.Essentials;

    public class Settings : INotifyPropertyChanged
    {
        static Settings settings;

        public static Settings Current => settings ?? (settings = new Settings());

        public void LeaveConferenceFeedback()
        {
            Preferences.Set("conferencefeedback_finished", true);
        }

        static readonly bool HasSetReminderDefault = false;

        static readonly bool ShowAllCategoriesDefault = true;

        public bool ShowAllCategories
        {
            get => Preferences.Get(nameof(ShowAllCategories), ShowAllCategoriesDefault);

            set => Preferences.Set(nameof(ShowAllCategories), value);
        }

        static readonly string FilteredCategoriesDefault = string.Empty;


        public string FilteredCategories
        {
            get => Preferences.Get(nameof(this.FilteredCategories), FilteredCategoriesDefault);

            set => Preferences.Set(nameof(this.FilteredCategories), value);
        }

        private bool isConnected;

        public bool IsConnected
        {
            get => this.isConnected;

            set
            {
                if (this.isConnected == value)
                {
                    return;
                }
                this.isConnected = value;
                this.OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged([CallerMemberName] string name = "") =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion
    }
}
