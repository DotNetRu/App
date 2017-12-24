namespace XamarinEvolve.Clients.Portable
{
    using System.Windows.Input;

    using MvvmHelpers;

    public class MenuItem : ObservableObject
    {
        private string name;

        private string subtitle;

        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        public string Subtitle
        {
            get => this.subtitle;
            set => this.SetProperty(ref this.subtitle, value);
        }

        public string Icon {get; set; }

        public string Parameter {get; set; }

        public AppPage Page { get; set; }

        public ICommand Command {get; set; }
    }
}

