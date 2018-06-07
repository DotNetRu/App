namespace DotNetRu.Utils.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public class Settings : INotifyPropertyChanged
    {
        static ISettings AppSettings => CrossSettings.Current;

        static Settings settings;

        public static Settings Current => settings ?? (settings = new Settings());

        public void LeaveConferenceFeedback()
        {
            AppSettings.AddOrUpdateValue("conferencefeedback_finished", true);
        }

        static readonly bool HasSetReminderDefault = false;


        static readonly bool ShowAllCategoriesDefault = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to show all categories.
        /// </summary>
        /// <value><c>true</c> if show all categories; otherwise, <c>false</c>.</value>
        public bool ShowAllCategories
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowAllCategories), ShowAllCategoriesDefault);

            set
            {
                if (AppSettings.AddOrUpdateValue(nameof(ShowAllCategories), value))
                {
                    this.OnPropertyChanged();
                }
            }
        }

        static readonly string FilteredCategoriesDefault = string.Empty;


        public string FilteredCategories
        {
            get => AppSettings.GetValueOrDefault(nameof(this.FilteredCategories), FilteredCategoriesDefault);

            set
            {
                if (AppSettings.AddOrUpdateValue(nameof(this.FilteredCategories), value))
                {
                    this.OnPropertyChanged();
                }
            }
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